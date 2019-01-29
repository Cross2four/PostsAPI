using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationPrincipal
{
    public static class ApplicationPrincipal
    {
        private static Func<IPrincipal> _current = () => Thread.CurrentPrincipal;


        public static IPrincipal Current
        {
            get { return _current(); }
        }

        public static void SwitchCurrentPrincipal(Func<IPrincipal> principal)
        {
            _current = principal;
        }

    }
}
