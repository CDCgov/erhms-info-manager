﻿using System.Data;

namespace ERHMS.Dapper
{
    public class LoggingConnection : IDbConnection
    {
        private IDbConnection @base;

        public string ConnectionString
        {
            get { return @base.ConnectionString; }
            set { @base.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return @base.ConnectionTimeout; }
        }

        public string Database
        {
            get { return @base.Database; }
        }

        public ConnectionState State
        {
            get { return @base.State; }
        }

        public LoggingConnection(IDbConnection @base)
        {
            this.@base = @base;
        }

        public IDbTransaction BeginTransaction()
        {
            return @base.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return @base.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            @base.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            @base.Close();
        }

        public IDbCommand CreateCommand()
        {
            return new LoggingCommand(@base.CreateCommand());
        }

        public void Dispose()
        {
            @base.Dispose();
        }

        public void Open()
        {
            @base.Open();
        }
    }
}
