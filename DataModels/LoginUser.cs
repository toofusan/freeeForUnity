using System;

namespace Freee
{

    [Serializable]
    public class LoginUser
    {
        public int id;
        public LoginUserCompany[] companies;
    }

    [Serializable]
    public class LoginUserCompany
    {
        public int id;
        public string name;
        public string role;
        public int employee_id;
    }
}