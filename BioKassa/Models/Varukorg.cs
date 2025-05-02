namespace BioKassa.Models;

public class VarukorgItem
{
    public Produkt Produkt { get; set; } = new Produkt();
    public int Antal { get; set; }
    public decimal Summa => Produkt.Pris * Antal;
}

public class Varukorg
{
    public List<VarukorgItem> Items { get; set; } = new List<VarukorgItem>();
    public decimal Totalsumma => Items.Sum(item => item.Summa);

    public void LäggTill(Produkt produkt)
    {
        var befintligItem = Items.FirstOrDefault(i => i.Produkt.Id == produkt.Id);
        if (befintligItem != null)
        {
            befintligItem.Antal++;
        }
        else
        {
            Items.Add(new VarukorgItem { Produkt = produkt, Antal = 1 });
        }
    }

    public void TaBort(Produkt produkt)
    {
        var befintligItem = Items.FirstOrDefault(i => i.Produkt.Id == produkt.Id);
        if (befintligItem != null)
        {
            befintligItem.Antal--;
            if (befintligItem.Antal <= 0)
            {
                Items.Remove(befintligItem);
            }
        }
    }

    public void Rensa()
    {
        Items.Clear();
    }
}