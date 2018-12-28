using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyClient.Controller
{
    using RubyClient.Models;

    public class SSController
    {
        private static SSController _instance;
        private static object syncRoot = new Object();

        private SSController() { }

        public static SSController GetInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new SSController();
                }
            }
            return _instance;
        }



    }
}
