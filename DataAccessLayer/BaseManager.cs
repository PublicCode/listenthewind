using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using DataAccess;
using DataAccess.DC;
using Newtonsoft.Json;
using ComLib.Extension;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace DataAccessLayer
{
    /// <summary>
    /// Base class for database imp.
    /// </summary>
    public class BaseManager
    {
        public DC dc;
        protected User _user;
        private readonly object _saveLock = new object();

        public BaseManager()
        {
            dc = DCLoader.GetMyDC();
        }

        private string Transform(Type objectType, string propertyName, object value)
        {
            bool needShort = (TypeDescriptor.GetProperties(objectType)[propertyName].Attributes[typeof(HistoryAttribute)] as HistoryAttribute).needConvertToShortDate;
            bool isCheckBox = (TypeDescriptor.GetProperties(objectType)[propertyName].Attributes[typeof(HistoryAttribute)] as HistoryAttribute).isCheckBox;
            if (isCheckBox)
            {
                if (value != null)
                {
                    if (value.ToString().ToLower() == "true" || value.ToString() == "1")
                        return "Checked";
                    else
                        return "Unchecked";
                }
                else
                    return "";
            }
            else
                return value == null ? "" : (needShort == true? DateTime.Parse(value.ToString()).ToShortDateString():value.ToString());
        }

        private bool Skip(PropertyInfo objectType, string propertyName)
        {
            if (objectType.GetCustomAttributes(typeof(HistoryAttribute), false).Length > 0)
                return false;
            else
                return true;
        }

        private bool Skip(object changeFrom)
        {
            if (changeFrom!= null && (changeFrom.GetType() == typeof(decimal) || changeFrom.GetType() == typeof(int)))
            {
                if (decimal.Parse(changeFrom.ToString()) > 0)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public void Save(User user = null, string saveType = "")
        {
            const string templAdd = "Added by {0} @ {1}.";
            const string templAddProperty = "{0}: {1}";
            const string templUpdate = "Updated by {0} @ {1}: \n\r ";
            const string templUpdateProperty = "{0}: from {1} to {2}";
            var @now = DateTime.UtcNow;

            if (user == null)
                user = new User { Name = "anonymous user" };
            var ChangeTracker = dc.ChangeTracker;
            var entitiesToAdd = new List<ILoggedEntity>(ChangeTracker.Entries<ILoggedEntity>().Count());

            var changedLoggedEntities = ChangeTracker.Entries<ILoggedEntity>();
            ModelAttrExtension modelAttr = new ModelAttrExtension();

            List<UserOperationLog> listOpLog = new List<UserOperationLog>();
            lock (_saveLock)
            {
                using (var ts = new TransactionScope())
                {
                    var batch = Guid.NewGuid();
                    var loggedEntity = changedLoggedEntities.ToList();
                    #region ILoggedEntity
                    foreach (var entry in loggedEntity)
                    {
                        var entryType = entry.Entity.GetType();

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                {
                                    saveType = saveType == "" ? "Added {0}" : saveType;
                                    //If entity state is add then add it and get the ID back
                                    int res = dc.SaveChanges();
                                    var ls = new List<string>();
                                    ls.Add(string.Format(templAdd, user.Name, now.ToString("yyyy/MM/dd H:mm:ss")));
                                    ls.AddRange(entry.CurrentValues.PropertyNames.Select(p => string.Format(templAddProperty, p,entry.CurrentValues[p])));
                                    var changeList = from c in entry.CurrentValues.PropertyNames select c;
                                    entitiesToAdd.Add(entry.Entity);
                                    //entry.Entity.OID = Guid.NewGuid();
                                    var log = listOpLog.FirstOrDefault(m => m.LogID == batch);
                                    if (log == null)
                                    {
                                        log = new UserOperationLog
                                        {
                                            LogID = batch,
                                            OperationTime = now,
                                            CreationTime = now,
                                            OID = entry.Entity.Id.ToString(),
                                            OperationType = entry.Entity.LoggedType,
                                            OperationInfo = string.Format(saveType, entry.Entity.LoggedType),
                                            OperationUserName = user.Name,
                                            OperationUserID = user.UserID,
                                            OperationDetails = new List<UserOperationDetail>()
                                        };
                                    }
  
                                    listOpLog.Add(log);
                                }
                                break;

                            case EntityState.Modified:
                                {
                                    var ls = new List<string>();
                                    ls.Add(string.Format(templUpdate, user.Name, now.ToString("yyyy/MM/dd H:mm:ss")));
                                    saveType = saveType == "" ? "Updated {0}" : saveType;
                                    var changeList = from c in entry.CurrentValues.PropertyNames
                                                     where entry.OriginalValues[c] == null ? entry.CurrentValues[c] != null : !entry.OriginalValues[c].Equals(entry.CurrentValues[c])
                                                     select new
                                                     {
                                                         fieldName = c,
                                                         From = entry.OriginalValues[c],
                                                         To = entry.CurrentValues[c],
                                                     };
                                    var log = listOpLog.FirstOrDefault(m => m.LogID == batch);
                                    if (log == null)
                                    {
                                        log = new UserOperationLog
                                        {
                                            LogID = batch,
                                            OID = entry.Entity.Id.ToString(),
                                            OperationTime = now,
                                            CreationTime = now,
                                            OperationType = entry.Entity.LoggedType,
                                            OperationInfo = string.Format(saveType, entry.Entity.LoggedType),
                                            OperationUserName = user.Name,
                                            OperationUserID = user.UserID,
                                            OperationDetails = new List<UserOperationDetail>()
                                        };
                                    }
                                    foreach (var change in changeList)
                                    {
                                        if (Skip(entry.Entity.GetType().GetProperty(change.fieldName), change.fieldName)) continue;
                                        ls.Add(string.Format(templUpdateProperty, change.fieldName, change.From, change.To));
                                        //Sometimes even it's the same but still in this list.
                                        if (change.From != change.To && Skip(change.From))
                                        {
                                            log.OperationDetails.Add(
                                                 new UserOperationDetail
                                                 {
                                                     LogID = batch,
                                                     ObjectField = modelAttr.ModelAttr(entry.Entity.GetType(), change.fieldName) == "" ? change.fieldName : modelAttr.ModelAttr(entry.Entity.GetType(), change.fieldName),
                                                     //ObjectField = change.fieldName,
                                                     ObjectType = entry.Entity.LoggedType,
                                                     ChangeType = "Update",
                                                     ChangeFrom = Transform(entry.Entity.GetType(), change.fieldName,entry.OriginalValues[change.fieldName]),
                                                     ChangeTo = Transform(entry.Entity.GetType(), change.fieldName,entry.CurrentValues[change.fieldName]),
                                                 });
                                        }
                                    }
                                    listOpLog.Add(log);
                                }
                                break;

                            case EntityState.Deleted:
                                {
                                    var log = listOpLog.FirstOrDefault(m => m.LogID == batch);
                                    if (log == null)
                                    {
                                        log = new UserOperationLog
                                        {
                                            LogID = batch,
                                            OperationTime = now,
                                            CreationTime = now,
                                            OperationType = entry.Entity.LoggedType,
                                            OperationInfo = string.Format("Deleted {0}", entry.Entity.LoggedType),
                                            OperationUserName = user.Name,
                                            OperationUserID = user.UserID,
                                            OperationDetails = new List<UserOperationDetail>()
                                        };
                                    }


                                    listOpLog.Add(log);
                                }

                                break;
                        }
                    }
                    #endregion

                    ts.Complete();
                }

                try
                {
                    int res = dc.SaveChanges();

                    for (int i = 0; i < listOpLog.Count; ++i)
                    {
                        dc.UserOperationLogs.Add(listOpLog[i]);
                    }
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

    }
}
