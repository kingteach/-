using Ams;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel;

namespace Service
{
    public class AuditService
    {
        public PagerInfo<TransMainInfo> Get(TransMainInfo condition, PagerInfo<TransMainInfo> pagerInfo)
        {
            using (AMSEntities ctx = new AMSEntities())
            {
                var query = from p in ctx.TRANS_MAIN

                            select new TransMainInfo()
                            {
                                Id = p.ID,
                                TransName = p.TRANSNAME,
                                CreaterName = p.CREATER,
                                CreateTime = p.CREATTIME,
                            };
                return ctx.GetByPage<TransMainInfo>(query, pagerInfo);
            }
        }
    }
}
