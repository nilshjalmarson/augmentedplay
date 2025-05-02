namespace BioKassa.Models;

public class Produkt
{
    public int Id { get; set; }
    public string Namn { get; set; } = string.Empty;
    public decimal Pris { get; set; }
    public ProduktTyp Typ { get; set; }
    public string Bild { get; set; } = string.Empty;
    public DryckTyp? DryckTyp { get; set; }
    public DryckStorlek? DryckStorlek { get; set; }
}

public enum ProduktTyp
{
    Biljett,
    Popcorn,
    Godis,
    Chips,
    Dryck
}

public enum DryckTyp
{
    Cola,
    Fanta,
    Äppeljuice
}

public enum DryckStorlek
{
    Liten,
    Mellan,
    Stor
}