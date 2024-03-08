namespace ProjectManager.Domain.Entities
{  
    public enum ProjetoStatus : short
    {
        Analise = 0, 
        AnaliseRealizada= 1, 
        AnaliseAprovada = 2, 
        Iniciado = 3,
        Planejado = 4, 
        Andamento = 5, 
        Encerrado = 6, 
        Cancelado = 7,
    }
}