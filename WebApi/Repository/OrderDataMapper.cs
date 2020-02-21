using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Repository
{
    public class OrderDataMapper: GenericDataMapper<Entity.Pedidos>
    {
        public OrderDataMapper(Entity.TestEntities Context) : base(Context) { }
    }
}