﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using nightowlsign.data.Interfaces;

namespace nightowlsign.data
{
    public class DataModule : Module
    {
        private string connStr;
        public DataModule(string connString)
        {
            this.connStr = connString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            //builder.Register(c => new DbContext(this.connStr)).As<IDbContext>().InstancePerRequest();

           // builder.RegisterType<SqlRepository>().As<IRepository>().InstancePerRequest();
          //  builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerRequest();
           // builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            base.Load(builder);
        }
    }

 
}
