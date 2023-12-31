﻿using Frontend.Models.ViewModel.Login;
using Newtonsoft.Json;

namespace Frontend.Utilities
{
    public class Common
    {
        IHttpContextAccessor _contxt = new HttpContextAccessor();
        public LoginVIewModel GetValueBySession()
        {
            var session = new LoginVIewModel();
            string sessionString = _contxt.HttpContext.Session.GetString(Constants.SessionKey.sessionLogin);

            if (sessionString != null)
                session = JsonConvert.DeserializeObject<LoginVIewModel>(sessionString);

            return session;
        }

        public class ddlValue
        {
            public string CODE { get; set; }
            public string TEXT { get; set; }
        }

        public string FormattedAmount(decimal value)
        {
            return value == null ? "null" : string.Format("{0:C}", value);
        }

        public List<ddlValue> GetListStatus()
        {
            List<ddlValue> list = new List<ddlValue>();
            ddlValue item = new ddlValue();
            try
            {

                item.CODE = "A";
                item.TEXT = "Active";
                list.Add(item);

                item = new ddlValue();
                item.CODE = "I";
                item.TEXT = "Inactive";
                list.Add(item);

                return list.ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
