using FluentMigrator;

namespace ProjectManager.Db.Migrations
{
    [Migration(20240303114400)]
    public class _20240303114400_CriarTabelaDeProjeto : Migration
    {
        internal static string TableName = "Projeto";

        public override void Up()
        {
            Create.Table(TableName)
             .WithColumn("Id").AsDecimal(18, 0).NotNullable().PrimaryKey()
             .WithColumn("Nome").AsString(150).NotNullable()
             .WithColumn("Descricao").AsString(1000).NotNullable()
             .WithColumn("DataInicio").AsDateTime().NotNullable()
             .WithColumn("DataTermino").AsDateTime().NotNullable()
             .WithColumn("DataCancelado").AsDateTime().Nullable()
             .WithColumn("Status").AsInt16().NotNullable()
             .WithColumn("Risco").AsInt16().Nullable()
             .WithColumn("ResponsavelId").AsDecimal(18, 0).NotNullable()
                    .ForeignKey("FK_" + TableName + "_ResponsavelId_Responsavel", _20240303100554_CriarTabelaDeResponsavel.TableName, "Id")
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
