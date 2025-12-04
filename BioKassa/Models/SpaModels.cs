using System;
using System.Collections.Generic;

namespace BioKassa.Models;

public enum BehandlingTyp
{
    Ansiktsbehandling,
    Massage,
    Bubbelpool,
    Pedikyr,
    Manikyr,
    Hårbehandling
}

public class Behandling
{
    public int Id { get; set; }
    public string Namn { get; set; } = string.Empty;
    public BehandlingTyp Typ { get; set; }
    public decimal Pris { get; set; }
    public TimeSpan Varaktighet { get; set; }
    public string Beskrivning { get; set; } = string.Empty;
    public string Ikon { get; set; } = string.Empty;
}

public class SpaAnställd
{
    public int Id { get; set; }
    public string Namn { get; set; } = string.Empty;
    public string Roll { get; set; } = string.Empty;
    public List<BehandlingTyp> Kompetenser { get; set; } = new();
    public string Avatar { get; set; } = string.Empty;
}

public class SpaBooking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Behandling Behandling { get; set; } = new();
    public SpaAnställd Anställd { get; set; } = new();
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Pris { get; set; }
    public string Kundnamn { get; set; } = string.Empty;
}

public class SpaKassaItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public Behandling Behandling { get; init; } = new();
    public SpaAnställd Anställd { get; init; } = new();
    public DateTime Start { get; init; }
    public DateTime Slut => Start + Behandling.Varaktighet;
    public decimal Pris => Behandling.Pris;
    public string Kundnamn { get; init; } = string.Empty;
}
