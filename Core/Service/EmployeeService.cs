using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel;

namespace Service
{
    public class EmployeeService
    {
        public List<Employee> GetByUnitId(string unitId, string name)
        {
            using (AMSEntities ctx = new AMSEntities())
            {
                var query = from p in ctx.SYS_TELLER_INFO
                            where p.UNIT_ID == unitId
                            select new Employee()
                            {
                                Id = p.TELLER_ID,
                                Name = p.CUST_NAME,
                                Duty = p.DUTY,
                                Station = p.STATION
                            };
                if (!string.IsNullOrEmpty(name))
                {
                    query.Where(x => x.Name.StartsWith(name));
                }
                return query.ToList();
            }
        }
    }
}
