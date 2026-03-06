using System;
using System.Collections.Generic;
using System.Linq;
using BioKassa.Models;

namespace BioKassa.Services;

public class SpaBookingService
{
    private static readonly TimeOnly Öppningstid = new(10, 0);
    private static readonly TimeOnly Stängningstid = new(18, 0);
    private static readonly TimeSpan SlotSteg = TimeSpan.FromMinutes(30);

    private readonly List<Behandling> _behandlingar = new()
    {
        new Behandling
        {
            Id = 101,
            Namn = "Ansiktsbehandling",
            Typ = BehandlingTyp.Ansiktsbehandling,
            Pris = 650,
            Varaktighet = TimeSpan.FromMinutes(60),
            Beskrivning = "Djuprengörande kur med mask och massage.",
            Ikon = "💆",
            IkonBild = "/images/spa-ansiktsbehandling.svg",
            Betyg = 5
        },
        new Behandling
        {
            Id = 102,
            Namn = "Massage",
            Typ = BehandlingTyp.Massage,
            Pris = 550,
            Varaktighet = TimeSpan.FromMinutes(45),
            Beskrivning = "Avslappnande helkroppsmassage.",
            Ikon = "💪",
            IkonBild = "/images/spa-massage.svg",
            Betyg = 5
        },
        new Behandling
        {
            Id = 103,
            Namn = "Bubbelpool",
            Typ = BehandlingTyp.Bubbelpool,
            Pris = 400,
            Varaktighet = TimeSpan.FromMinutes(30),
            Beskrivning = "Privat bubbelpool med doftljus.",
            Ikon = "🫧",
            IkonBild = "/images/spa-bubbelpool.svg",
            Betyg = 4
        },
        new Behandling
        {
            Id = 104,
            Namn = "Pedikyr",
            Typ = BehandlingTyp.Pedikyr,
            Pris = 520,
            Varaktighet = TimeSpan.FromMinutes(50),
            Beskrivning = "Fotbad, peeling och lack.",
            Ikon = "🦶",
            IkonBild = "/images/spa-pedikyr.svg",
            Betyg = 4
        },
        new Behandling
        {
            Id = 105,
            Namn = "Manikyr",
            Typ = BehandlingTyp.Manikyr,
            Pris = 480,
            Varaktighet = TimeSpan.FromMinutes(45),
            Beskrivning = "Formning, lack och handmassage.",
            Ikon = "💅",
            IkonBild = "/images/spa-manikyr.svg",
            Betyg = 5
        },
        new Behandling
        {
            Id = 106,
            Namn = "Hårbehandling",
            Typ = BehandlingTyp.Hårbehandling,
            Pris = 620,
            Varaktighet = TimeSpan.FromMinutes(60),
            Beskrivning = "Inpackning och styling.",
            Ikon = "💇",
            IkonBild = "/images/spa-harBehandling.svg",
            Betyg = 4
        }
    };

    private readonly List<SpaAnställd> _anställda = new()
    {
        new SpaAnställd
        {
            Id = 1,
            Namn = "Liv",
            Roll = "Behandlare",
            Avatar = "🌸",
            AvatarBild = "/images/personal-liv.svg",
            Kompetenser = new()
        },
        new SpaAnställd
        {
            Id = 2,
            Namn = "Benjamin",
            Roll = "Behandlare",
            Avatar = "😎",
            AvatarBild = "/images/personal-benjamin.svg",
            Kompetenser = new()
        },
        new SpaAnställd
        {
            Id = 3,
            Namn = "Anna",
            Roll = "Behandlare",
            Avatar = "💼",
            AvatarBild = "/images/personal-anna.svg",
            Kompetenser = new()
        },
        new SpaAnställd
        {
            Id = 4,
            Namn = "Nils",
            Roll = "Behandlare",
            Avatar = "🧔",
            AvatarBild = "/images/personal-nils.svg",
            Kompetenser = new()
        },
        new SpaAnställd
        {
            Id = 5,
            Namn = "Erik",
            Roll = "Behandlare",
            Avatar = "😄",
            AvatarBild = "/images/personal-erik.svg",
            Kompetenser = new()
        },
        new SpaAnställd
        {
            Id = 6,
            Namn = "Nevin",
            Roll = "Behandlare",
            Avatar = "⚡",
            AvatarBild = "/images/personal-nevin.svg",
            Kompetenser = new()
        }
    };

    private readonly List<SpaBooking> _bokadeTider = new();

    public IReadOnlyList<Behandling> HämtaBehandlingar() => _behandlingar;

    public IReadOnlyList<SpaAnställd> HämtaAllaAnställda() => _anställda;

    public IReadOnlyList<SpaBooking> HämtaBokningarFörDag(DateOnly datum)
    {
        return _bokadeTider
            .Where(b => DateOnly.FromDateTime(b.Start) == datum)
            .OrderBy(b => b.Start)
            .ToList();
    }

    public IReadOnlyList<SpaAnställd> HämtaAnställdaFörBehandling(int behandlingId)
    {
        var behandling = _behandlingar.FirstOrDefault(b => b.Id == behandlingId);
        if (behandling is null)
        {
            return Array.Empty<SpaAnställd>();
        }

        return _anställda
            .Where(a => a.Kompetenser.Contains(behandling.Typ))
            .ToList();
    }

    public IReadOnlyList<DateTime> HämtaTillgängligaSlots(DateOnly datum, int behandlingId, int anställdId)
    {
        var behandling = _behandlingar.First(b => b.Id == behandlingId);
        var startPåDag = datum.ToDateTime(Öppningstid);
        var slutPåDag = datum.ToDateTime(Stängningstid);

        var resultat = new List<DateTime>();
        for (var start = startPåDag; start + behandling.Varaktighet <= slutPåDag; start += SlotSteg)
        {
            var slut = start + behandling.Varaktighet;
            if (ÄrSlotLedig(anställdId, start, slut))
            {
                resultat.Add(start);
            }
        }

        return resultat;
    }

    public void BekräftaBokningar(IEnumerable<SpaKassaItem> items)
    {
        var lista = items.ToList();
        foreach (var item in lista)
        {
            if (!ÄrSlotLedig(item.Anställd.Id, item.Start, item.Slut))
            {
                throw new InvalidOperationException($"Tiden {item.Start:t} är redan bokad för {item.Anställd.Namn}.");
            }
        }

        foreach (var item in lista)
        {
            _bokadeTider.Add(new SpaBooking
            {
                Id = Guid.NewGuid(),
                Behandling = item.Behandling,
                Anställd = item.Anställd,
                Start = item.Start,
                End = item.Slut,
                Pris = item.Pris,
                Kundnamn = item.Kundnamn
            });
        }
    }

    public SpaAnställd LäggTillAnställd(string namn, string roll, IEnumerable<BehandlingTyp> kompetenser, string avatar)
    {
        if (string.IsNullOrWhiteSpace(namn))
        {
            throw new ArgumentException("Namn måste anges", nameof(namn));
        }

        var kompetensLista = kompetenser?.Distinct().ToList() ?? new List<BehandlingTyp>();
        if (!kompetensLista.Any())
        {
            throw new ArgumentException("Minst en behandlingstyp måste väljas", nameof(kompetenser));
        }

        var nyttId = _anställda.Any() ? _anställda.Max(a => a.Id) + 1 : 1;
        var anställd = new SpaAnställd
        {
            Id = nyttId,
            Namn = namn.Trim(),
            Roll = string.IsNullOrWhiteSpace(roll) ? "Behandlare" : roll.Trim(),
            Avatar = string.IsNullOrWhiteSpace(avatar) ? "🙂" : avatar.Trim(),
            Kompetenser = kompetensLista
        };

        _anställda.Add(anställd);
        return anställd;
    }

    public SpaAnställd UppdateraAnställd(int id, string namn, string roll, IEnumerable<BehandlingTyp> kompetenser, string avatar)
    {
        var anställd = _anställda.FirstOrDefault(a => a.Id == id)
            ?? throw new ArgumentException($"Ingen anställd med id {id} hittades", nameof(id));

        if (string.IsNullOrWhiteSpace(namn))
        {
            throw new ArgumentException("Namn måste anges", nameof(namn));
        }

        var kompetensLista = kompetenser?.Distinct().ToList() ?? new List<BehandlingTyp>();
        if (!kompetensLista.Any())
        {
            throw new ArgumentException("Minst en behandlingstyp måste väljas", nameof(kompetenser));
        }

        anställd.Namn = namn.Trim();
        anställd.Roll = string.IsNullOrWhiteSpace(roll) ? "Behandlare" : roll.Trim();
        anställd.Avatar = string.IsNullOrWhiteSpace(avatar) ? "🙂" : avatar.Trim();
        anställd.Kompetenser = kompetensLista;

        return anställd;
    }

    public void TaBortAnställd(int id)
    {
        var anställd = _anställda.FirstOrDefault(a => a.Id == id)
            ?? throw new ArgumentException($"Ingen anställd med id {id} hittades", nameof(id));

        _anställda.Remove(anställd);
        _bokadeTider.RemoveAll(b => b.Anställd.Id == id);
    }

    private bool ÄrSlotLedig(int anställdId, DateTime start, DateTime slut)
    {
        return !_bokadeTider.Any(b => b.Anställd.Id == anställdId && start < b.End && slut > b.Start);
    }

    public void SättBehandlingBetyg(int id, int betyg)
    {
        var behandling = _behandlingar.FirstOrDefault(b => b.Id == id);
        if (behandling != null) behandling.Betyg = betyg;
    }
}
