using FluentMigrator;
using System;

namespace ProjectManager.Db.Migrations
{
    [Migration(20240303100552)]
    public class _20240303100552_CriarTabelaDeSimNao : Migration
    {
        internal static string TableName = "SimNao";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey()
                .WithColumn("Descricao").AsString(10).NotNullable().WithDefaultValue(string.Empty)
                .WithColumn("ExcluidoId").AsInt16().NotNullable().Indexed()
                .WithColumn("IdExterno").AsGuid().NotNullable().Indexed();

            Insert.IntoTable(TableName).Row(new { Id = 1, Descricao = "Sim", ExcluidoId = 0, IdExterno = Guid.NewGuid() });
            Insert.IntoTable(TableName).Row(new { Id = 0, Descricao = "Não", ExcluidoId = 0, IdExterno = Guid.NewGuid() });
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}
