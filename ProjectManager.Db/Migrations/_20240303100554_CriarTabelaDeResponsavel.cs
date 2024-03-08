using FluentMigrator;

namespace ProjectManager.Db.Migrations
{
    [Migration(20240303100554)]
    public class _20240303100554_CriarTabelaDeResponsavel : Migration
    {
        internal static string TableName = "Responsavel";

        public override void Up()
        {
            Create.Table(TableName)
              .WithColumn("Id").AsDecimal(18, 0).NotNullable().PrimaryKey()
              .WithColumn("Nome").AsString(50).Nullable()
              .WithColumn("Sobrenome").AsString(100).Nullable()
              .WithColumn("Email").AsString(254).Nullable()
              .WithColumn("Codigo").AsString(100).NotNullable()
              .WithColumn("AtivoId").AsInt16().NotNullable()
                    .ForeignKey("FK_" + TableName + "_AtivoId_SimNao", _20240303100552_CriarTabelaDeSimNao.TableName, "Id")
                .WithColumn("ExcluidoId").AsInt16().NotNullable().Indexed()
                    .ForeignKey("FK_" + TableName + "_ExcluidoId_SimNao", _20240303100552_CriarTabelaDeSimNao.TableName, "Id")
              .WithColumn("IdExterno").AsGuid().NotNullable().Indexed();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}