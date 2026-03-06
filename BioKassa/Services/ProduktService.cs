using BioKassa.Models;

namespace BioKassa.Services;

public class ProduktService
{
    private readonly List<Produkt> _produkter = new()
    {
        // Biljetter
        new Produkt { Id = 1, Namn = "Vuxen", Pris = 129, Typ = ProduktTyp.Biljett, Bild = "images/ticket.png" },
        new Produkt { Id = 2, Namn = "Barn", Pris = 89, Typ = ProduktTyp.Biljett, Bild = "images/ticket-child.png" },
        new Produkt { Id = 3, Namn = "Pensionär", Pris = 99, Typ = ProduktTyp.Biljett, Bild = "images/ticket-senior.png" },
        
        // Popcorn
        new Produkt { Id = 4, Namn = "Liten Popcorn", Pris = 39, Typ = ProduktTyp.Popcorn, Bild = "images/popcorn-small.png", Betyg = 3 },
        new Produkt { Id = 5, Namn = "Mellan Popcorn", Pris = 49, Typ = ProduktTyp.Popcorn, Bild = "images/popcorn-medium.png", Betyg = 4 },
        new Produkt { Id = 6, Namn = "Stor Popcorn", Pris = 59, Typ = ProduktTyp.Popcorn, Bild = "images/popcorn-large.png", Betyg = 5 },
        
        // Godis
        new Produkt { Id = 7, Namn = "Choklad", Pris = 25, Typ = ProduktTyp.Godis, Bild = "images/candy-chocolate.png", Betyg = 4 },
        new Produkt { Id = 8, Namn = "Gelégodis", Pris = 25, Typ = ProduktTyp.Godis, Bild = "images/candy-gummy.png", Betyg = 5 },
        new Produkt { Id = 9, Namn = "Lakrits", Pris = 25, Typ = ProduktTyp.Godis, Bild = "images/candy-licorice.png", Betyg = 3 },
        
        // Chips
        new Produkt { Id = 10, Namn = "Salted", Pris = 35, Typ = ProduktTyp.Chips, Bild = "images/chips-salted.png", Betyg = 3 },
        new Produkt { Id = 11, Namn = "Sourcream & Onion", Pris = 35, Typ = ProduktTyp.Chips, Bild = "images/chips-sour.png", Betyg = 4 },
        new Produkt { Id = 12, Namn = "Grillchips", Pris = 35, Typ = ProduktTyp.Chips, Bild = "images/chips-bbq.png", Betyg = 5 },
        
        // Drycker - Cola
        new Produkt { Id = 13, Namn = "Liten Cola", Pris = 25, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Cola, DryckStorlek = DryckStorlek.Liten, Bild = "images/drink-cola-small.png", Betyg = 4 },
        new Produkt { Id = 14, Namn = "Mellan Cola", Pris = 35, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Cola, DryckStorlek = DryckStorlek.Mellan, Bild = "images/drink-cola-medium.png", Betyg = 4 },
        new Produkt { Id = 15, Namn = "Stor Cola", Pris = 45, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Cola, DryckStorlek = DryckStorlek.Stor, Bild = "images/drink-cola-large.png", Betyg = 5 },
        
        // Drycker - Fanta
        new Produkt { Id = 16, Namn = "Liten Fanta", Pris = 25, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Fanta, DryckStorlek = DryckStorlek.Liten, Bild = "images/drink-fanta-small.png", Betyg = 3 },
        new Produkt { Id = 17, Namn = "Mellan Fanta", Pris = 35, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Fanta, DryckStorlek = DryckStorlek.Mellan, Bild = "images/drink-fanta-medium.png", Betyg = 4 },
        new Produkt { Id = 18, Namn = "Stor Fanta", Pris = 45, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Fanta, DryckStorlek = DryckStorlek.Stor, Bild = "images/drink-fanta-large.png", Betyg = 4 },
        
        // Drycker - Äppeljuice
        new Produkt { Id = 19, Namn = "Liten Äppeljuice", Pris = 25, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Äppeljuice, DryckStorlek = DryckStorlek.Liten, Bild = "images/drink-apple-small.png", Betyg = 3 },
        new Produkt { Id = 20, Namn = "Mellan Äppeljuice", Pris = 35, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Äppeljuice, DryckStorlek = DryckStorlek.Mellan, Bild = "images/drink-apple-medium.png", Betyg = 4 },
        new Produkt { Id = 21, Namn = "Stor Äppeljuice", Pris = 45, Typ = ProduktTyp.Dryck, DryckTyp = DryckTyp.Äppeljuice, DryckStorlek = DryckStorlek.Stor, Bild = "images/drink-apple-large.png", Betyg = 5 }
    };

    public List<Produkt> HämtaAllaProdukter() => _produkter;

    public List<Produkt> HämtaProdukterEfterTyp(ProduktTyp typ) => 
        _produkter.Where(p => p.Typ == typ).ToList();
        
    public List<Produkt> HämtaDryckerEfterTyp(DryckTyp dryckTyp) =>
        _produkter.Where(p => p.Typ == ProduktTyp.Dryck && p.DryckTyp == dryckTyp).ToList();

    public Produkt? HämtaProduktMedId(int id) => 
        _produkter.FirstOrDefault(p => p.Id == id);

    public void SättBetyg(int id, int betyg)
    {
        var produkt = _produkter.FirstOrDefault(p => p.Id == id);
        if (produkt != null) produkt.Betyg = betyg;
    }
}