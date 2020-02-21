using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Repository
{
    public class ClientDataMapper: GenericDataMapper<Entity.Clientes>
    {
        public ClientDataMapper(Entity.TestEntities Context) : base(Context) { }
    }
}