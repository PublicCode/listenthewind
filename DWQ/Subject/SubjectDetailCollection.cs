using System;
using System.Collections.Generic;

namespace DWQ.Subject
{
    public class SubjectDetailCollection
    {
        private List<SubjectDetailInfo> _details = new List<SubjectDetailInfo>();
        public SubjectDetailInfo this[int i]
        {
            get
            {
                return _details[i];
            }
        }

        private Dictionary<string, SubjectDetailInfo> _bindDetails = new Dictionary<string, SubjectDetailInfo>();
        public SubjectDetailInfo this[string fieldName]
        {
            get
            {
                return _bindDetails[fieldName];
            }
        }

        public int Count
        {
            get
            {
                return this._details.Count;
            }
        }

        public void Add(SubjectDetailInfo subjectDetail)
        {
            try
            {
                _details.Add(subjectDetail);
                _bindDetails.Add(subjectDetail.FieldName, subjectDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(int index)
        {
            if (index > _details.Count - 1 || index < 0)
            {
                throw new Exception("Index not valid!");
            }
            else
            {
                _details.RemoveAt(index);
            }
        }

        public System.Collections.IEnumerable GetEnumerator()
        {
            for (int i = 0; i < _details.Count; i++)
            {
                yield return _details[i];
            }
        }
    }
}
