using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1PC
{
    class RendezvousChannel<S,R>
    {
        public bool Request(S service,int timeout, out R response)
        {
        
        }

    public  object Accept(int timeout, out S service)
        {
            
        }


        void Reply(object rendezVousToken, R Response)
        {
            
        }
    }
}
