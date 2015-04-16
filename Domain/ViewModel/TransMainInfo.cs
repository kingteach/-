using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModel
{
    public class TransMainInfo
    {
        //ID, TRANSID, TRANSNAME, CREATEID, CREATER, CREATTIME, DESCRIPTION, STATU
        public string Id { get; set; }

        public string TransId { get; set; }

        public string TransName { get; set; }

        public string CreateId { get; set; }

        public string CreaterName { get; set; }

        public DateTime CreateTime { get; set; }

        public string Description { get; set; }

        public Statu Statu { get; set; }
        public string StatuMc { get { return this.Statu.GetDesc(); } }
    }
}
