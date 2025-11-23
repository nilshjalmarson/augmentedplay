using System;
using System.Collections.Generic;
using System.Linq;
using BioKassa.Models;

namespace BioKassa.Services;

public class SpaKassa
{
    private readonly List<SpaKassaItem> _items = new();

    public IReadOnlyList<SpaKassaItem> Items => _items;

    public decimal Totalsumma => _items.Sum(item => item.Pris);

    public void LäggTill(SpaKassaItem item)
    {
        if (_items.Any(i => i.Anställd.Id == item.Anställd.Id && TiderKrockar(i.Start, i.Slut, item.Start, item.Slut)))
        {
            throw new InvalidOperationException("Den valda tiden krockar med en annan bokning i kassan.");
        }

        _items.Add(item);
    }

    public void TaBort(Guid id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public void Rensa() => _items.Clear();

    private static bool TiderKrockar(DateTime startA, DateTime slutA, DateTime startB, DateTime slutB)
    {
        return startA < slutB && slutA > startB;
    }
}
