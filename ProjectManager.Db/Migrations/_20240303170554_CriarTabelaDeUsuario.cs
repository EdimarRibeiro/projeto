using FluentMigrator;

namespace ProjectManager.Db.Migrations
{
    [Migration(20240303170554)]
    public class _20240303170554_CriarTabelaDeUsuario : Migration
    {
        internal static string TableName = "Usuario";

        public override void Up()
        {
            Create.Table(TableName)
              .WithColumn("Id").AsDecimal(18, 0).NotNullable().PrimaryKey()
              .WithColumn("Login").AsString(254).Nullable().Indexed()
              .WithColumn("Senha").AsString(254).Nullable().Indexed()
              .WithColumn("Nome").AsString(254).Nullable()
              .WithColumn("Celular").AsString(12).Nullable()
              .WithColumn("FotoUrl").AsString(500).Nullable()
              .WithColumn("CnpjCpf").AsString(18).Nullable().Indexed()
              .WithColumn("AlterarSenha").AsInt16().NotNullable().WithDefaultValue(0)
                  .ForeignKey("FK_" + TableName + "_AlterarSenha_SimNao", _20240303100552_CriarTabelaDeSimNao.TableName, "Id")
              .WithColumn("DataCadastro").AsDateTime().NotNullable()
              .WithColumn("DataExpiracao").AsDateTime().Nullable()
              .WithColumn("EmailVerificado").AsInt16().NotNullable().WithDefaultValue(0)
                  .ForeignKey("FK_" + TableName + "_EmailVerificado_SimNao", _20240303100552_CriarTabelaDeSimNao.TableName, "Id")
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