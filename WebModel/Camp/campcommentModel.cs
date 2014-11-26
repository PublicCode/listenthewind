using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class campcommentModel
    {
        public int CampCommentID { get; set; }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string CommentCon { get; set; }

        public string CommentRes { get; set; }

        public DateTime? CommentTime { get; set; }

        public DateTime? ResponseTime { get; set; }
    }
}
