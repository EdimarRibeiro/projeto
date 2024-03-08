using FluentMigrator;

namespace ProjectManager.Db.Migrations
{
    [Migration(20240304114400)]
    public class _20240304114400_CriarTabelaDeProjetoResponsavel : Migration
    {
        internal static string TableName = "ProjetoResponsavel";

        public override void Up()
        {
            Create.Table(TableName)
             .WithColumn("Id").AsDecimal(18, 0).NotNullable().PrimaryKey()
             .WithColumn("ProjetoId").AsDecimal(18, 0).NotNullable()
                    .ForeignKey("FK_" + TableName + "_ProjetoId_Projeto", _20240303114400_CriarTabelaDeProjeto.TableName, "Id")
             .WithColumn("ResponsavelId").AsDecimal(18, 0).NotNullable().Indexed()
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
