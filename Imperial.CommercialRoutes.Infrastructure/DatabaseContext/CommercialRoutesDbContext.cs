using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Imperial.CommercialRoutes.Infrastructure.DatabaseContext
{
    /// <summary>
    /// Commercial routes DB context.
    /// </summary>
    public partial class CommercialRoutesDbContext
    {
        #region Fields

        private readonly string nameOrConnectionString;
        private const string DatabaseName = "CommercialRoute";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="CommercialRoutesDbContext"/>.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string</param>
        public CommercialRoutesDbContext(string nameOrConnectionString)
        {
            this.nameOrConnectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
        }

        #endregion

        #region Methods

        public IDbConnection Connection => new SqlConnection(nameOrConnectionString);

        public void Init()
        {
            InitDatabase();
            InitTables();
        }

        private void InitDatabase()
        {
            // create database if it does not exist
            using (IDbConnection connection = Connection)
            {
                string sql = string.Format(@"IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = '{0}')
                                BEGIN
                                    CREATE DATABASE {0};
                                END", arg0: DatabaseName);

                connection.Execute(sql);
            }
        }

        private void InitTables()
        {
            // create tables if it do not exist
            using (IDbConnection connection = Connection)
            {
                string planetTableSql = string.Format(
                    @"IF OBJECT_ID(N'[{0}].[dbo].[Planet]', N'U') IS NULL
                        BEGIN
                        CREATE TABLE [{0}].[dbo].[Planet](
                            [Id] [int] IDENTITY(1,1) NOT NULL,
                            [Name] [nvarchar](50) NOT NULL,
                            [Code] [varchar](3) NOT NULL,
                            [Sector] [varchar](2) NOT NULL,
                            [RebelInfluence] [int] NOT NULL,
                        CONSTRAINT [PK_Planet] PRIMARY KEY CLUSTERED
                        (
                            [Id] ASC
                        )
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY])
                        ON [PRIMARY]
                        END", arg0: DatabaseName);

                string distanceTableSql = string.Format(
                    @"IF OBJECT_ID(N'[{0}].[dbo].[Distance]', N'U') IS NULL
                        BEGIN
                        CREATE TABLE [{0}].[dbo].[Distance](
	                        [Id] [int] IDENTITY(1,1) NOT NULL,
	                        [Origin] [varchar](3) NOT NULL,
	                        [Destination] [varchar](3) NOT NULL,
	                        [LunarYears] [decimal](18, 2) NOT NULL,
                        CONSTRAINT [PK_Distance] PRIMARY KEY CLUSTERED 
                        (
	                        [Id] ASC
                        )
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY])
                        ON [PRIMARY]
                        END", arg0: DatabaseName);

                connection.Execute(planetTableSql);
                connection.Execute(distanceTableSql);
            }
        }

        #endregion
    }
}
