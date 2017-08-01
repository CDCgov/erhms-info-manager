﻿using Dapper;
using ERHMS.Dapper;
using ERHMS.Domain;
using ERHMS.EpiInfo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERHMS.DataAccess
{
    public class LinkRepository<TLink> : EntityRepository<TLink>
        where TLink : Link
    {
        public new DataContext Context { get; private set; }

        protected LinkRepository(DataContext context)
            : base(context)
        {
            Context = context;
        }

        public override IEnumerable<TLink> Select(string clauses = null, object parameters = null)
        {
            return Database.Invoke((connection, transaction) =>
            {
                SqlBuilder sql = new SqlBuilder();
                sql.AddTable(TypeMap.TableName);
                sql.AddSeparator();
                sql.AddTable(JoinType.Inner, "ERHMS_Incidents", "IncidentId", TypeMap.TableName);
                sql.OtherClauses = clauses;
                Func<TLink, Incident, TLink> map = (link, incident) =>
                {
                    link.New = false;
                    incident.New = false;
                    link.Incident = incident;
                    return link;
                };
                return connection.Query(sql.ToString(), map, parameters, transaction, splitOn: sql.SplitOn);
            });
        }

        public IEnumerable<TLink> SelectUndeleted()
        {
            return Select("WHERE [ERHMS_Incidents].[Deleted] = 0");
        }

        public override TLink SelectById(object id)
        {
            string clauses = string.Format("WHERE {0}.{1} = @Id", Escape(TypeMap.TableName), Escape(TypeMap.GetId().ColumnName));
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return Select(clauses, parameters).SingleOrDefault();
        }

        public IEnumerable<TLink> SelectByIncidentId(string incidentId)
        {
            string clauses = string.Format("WHERE {0}.[IncidentId] = @IncidentId", Escape(TypeMap.TableName));
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IncidentId", incidentId);
            return Select(clauses, parameters);
        }
    }
}