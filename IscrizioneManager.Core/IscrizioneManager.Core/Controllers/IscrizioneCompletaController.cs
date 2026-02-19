using IscrizioneManager.Core.Items;
using IscrizioneManager.Core.Services;
using IscrizioniManager;
using IscrizioniManager.Items;
using IscrizioniManager.Models;
using IscrizioniManager.Utils;

public class IscrizioneCompletaController
{
  public IscrizioneCompletaController()
  {
  }

  public async Task<bool> CreateAsync(ModuloIscrizioneDto dto)
  {
    Bambino bambinoCreato;
      // crea nuovo bambino
    var bambino = new Bambino
    {
      Nome = dto.Nome!,
      Cognome = dto.Cognome!,
      DataNascita = dto.DataNascita.Value.ToString("yyyy-MM-dd"),
      Genere = dto.Genere!.Value,
      LuogoNascita = dto.LuogoNascita,
      IndirizzoResidenza = dto.IndirizzoResidenza,
      ComuneResidenza = dto.ComuneResidenza
    };
    var bambinoResp = await ClientHolder.Client.GetAll<Bambino>().InsertAsync(bambino);
    bambinoCreato = bambinoResp.Models.First();

    // --- 2. Crea genitori e relazioni
    if (dto.Genitori != null)
    {
      foreach (var g in dto.Genitori)
      {
        Genitore genitoreCreato;

        if (g.IdGenitore.HasValue && g.IdGenitore.Value > 0)
        {
          // caso 1: genitore già esistente
          var existing = await ClientHolder.Client
              .GetAll<Genitore>()
              .Select("*")
              .Where(x => x.Id == g.IdGenitore.Value)
              .Single();

          if (existing == null)
            throw new Exception($"Genitore con Id {g.IdGenitore.Value} non trovato");

          genitoreCreato = existing;
        }
        else
        {
          // caso 2: nuovo genitore
          var nuovo = new Genitore
          {
            Nome = g.Nome!,
            Cognome = g.Cognome!,
            Telefono = g.Telefono,
            Sesso = g.Sesso,
          };
          var resp = await ClientHolder.Client.GetAll<Genitore>().InsertAsync(nuovo);
          genitoreCreato = resp.Models.First();
        }

        // --- crea relazione genitore-bambino
        var rel = new GenitoreBambino
        {
          IdGenitore = genitoreCreato.Id,
          IdBambino = bambinoCreato.Id,
        };
        await ClientHolder.Client.GetAll<GenitoreBambino>().InsertAsync(rel);

      }
    }

    // --- 3. Crea iscrizione
    var iscrizione = new Iscrizione
    {
      IdBambino = bambinoCreato.Id,
      Anno = (AnnoScolastico?)dto.AnnoScolastico,
      Note = dto.Note,
      DaIscrivereAlNoi = dto.DaIscrivereAlNoi,
      ModalitaPagamento = dto.ModalitaPagamentoSelezionata,
      FormatoIscrizione = dto.FormatoIscrizioneSelezionato,
      ScontoFratelli = dto.ScontoFratelli
    };
    var iscrizioneResp = await ClientHolder.Client.GetAll<Iscrizione>().InsertAsync(iscrizione);
    var iscrizioneCreato = iscrizioneResp.Models.First();

    // --- 4. Settimane
    if (dto.Settimane != null)
    {
      foreach (var idSettimana in dto.Settimane)
      {
        var iscrSettimana = new IscrizioneSettimana
        {
          IdIscrizione = iscrizioneCreato.Id,
          IdSettimana = idSettimana.CostoIntero != null ? idSettimana.Id : -idSettimana.Id,
          Intero = idSettimana.CostoIntero != null
        };
        await ClientHolder.Client.GetAll<IscrizioneSettimana>().InsertAsync(iscrSettimana);
      }
    }

    // --- 5. Taglia
    if (dto.Taglia != null)
    {
      var iscrTaglia = new IscrizioneTaglia
      {
        IdIscrizione = iscrizioneCreato.Id,
        IdTaglia = dto.Taglia.Value
      };
      await ClientHolder.Client.GetAll<IscrizioneTaglia>().InsertAsync(iscrTaglia);
    }

    // --- 6. Scheda sanitaria
    var scheda = new SchedaSanitaria
    {
      IdIscrizione = iscrizioneCreato.Id,
      AllergieIntolleranze = dto.AllergieIntolleranze,
      PatologieTerapie = dto.PatologieTerapie
    };
    await ClientHolder.Client.GetAll<SchedaSanitaria>().InsertAsync(scheda);

    // --- 7. Consensi
    if (dto.ConsensiDisponibili != null)
    {
      foreach (var c in dto.ConsensiDisponibili)
      {
        if (c.IdTipoConsenso.HasValue)
        {
          var consenso = new Consenso
          {
            IdIscrizione = iscrizioneCreato.Id,
            IdTipoConsenso = c.IdTipoConsenso.Value,
            Valore = c.IsSelected,
          };
          await ClientHolder.Client.GetAll<Consenso>().InsertAsync(consenso);
        }
      }
    }

    //return new ModuloIscrizioneDto
    //{
    //  IdBambino = bambinoCreato.Id,
    //  Nome = bambinoCreato.Nome,
    //  Cognome = bambinoCreato.Cognome,
    //  DataNascita = DateOnly.Parse(bambinoCreato.DataNascita),
    //  LuogoNascita = bambinoCreato.LuogoNascita,
    //  IndirizzoResidenza = bambinoCreato.IndirizzoResidenza,
    //  IdIscrizione = iscrizioneCreato.Id,
    //  Anno = iscrizioneCreato.Anno,
    //  Note = iscrizioneCreato.Note
    //  // Genitori, Settimane, Consensi ecc. → puoi caricarli se vuoi subito
    //};
    return true;
  }

  public async Task<bool> UpdateAsync(int idBambino, ModuloIscrizioneDto dto)
  {
    // --- Aggiorna dati del bambino
    if (dto.Nome != null || dto.Cognome != null || dto.DataNascita.HasValue || dto.Genere.HasValue ||
        dto.LuogoNascita != null || dto.IndirizzoResidenza != null || dto.ComuneResidenza != null)
    {
      var bambinoResp = await ClientHolder.Client
          .GetAll<Bambino>()
          .Select("*")
          .Where(x => x.Id == idBambino)
          .Get();

      var bambino = bambinoResp.Models?.FirstOrDefault();
      if (bambino == null) throw new Exception("Bambino non trovato");

      if (dto.Nome != null) bambino.Nome = dto.Nome;
      if (dto.Cognome != null) bambino.Cognome = dto.Cognome;
      if (dto.DataNascita.HasValue) bambino.DataNascita = dto.DataNascita.Value.ToString("yyyy-MM-dd");
      if (dto.Genere != null) bambino.Genere = dto.Genere.Value;
      if (dto.LuogoNascita != null) bambino.LuogoNascita = dto.LuogoNascita;
      if (dto.IndirizzoResidenza != null) bambino.IndirizzoResidenza = dto.IndirizzoResidenza;
      if (dto.ComuneResidenza != null) bambino.ComuneResidenza = dto.ComuneResidenza;

      await ClientHolder.Client.GetAll<Bambino>().Update(bambino);
    }

    if (dto.Genitori != null)
    {
      // Prendi tutte le relazioni esistenti
      var relazioniEsistenti = await ClientHolder.Client
          .GetAll<GenitoreBambino>()
          .Where(x => x.IdBambino == idBambino)
          .Get();

      // Cancella tutte le relazioni esistenti
      foreach (var rel in relazioniEsistenti.Models ?? new List<GenitoreBambino>())
      {
        await ClientHolder.Client.GetAll<GenitoreBambino>().Delete(rel);
      }

      foreach (var g in dto.Genitori)
      {
        Genitore genitoreAggiornato;

        if (g.IdGenitore.HasValue && g.IdGenitore.Value > 0)
        {
          // Aggiorna genitore esistente
          var existing = await ClientHolder.Client
              .GetAll<Genitore>()
              .Where(x => x.Id == g.IdGenitore.Value)
              .Single();

          if (existing == null)
            throw new Exception($"Genitore con Id {g.IdGenitore.Value} non trovato");

          existing.Nome = g.Nome;
          existing.Cognome = g.Cognome;
          existing.Telefono = g.Telefono;
          existing.Sesso = g.Sesso;

          await ClientHolder.Client.GetAll<Genitore>().Update(existing);
          genitoreAggiornato = existing;
        }
        else
        {
          // Inserisci nuovo genitore
          var nuovo = new Genitore
          {
            Nome = g.Nome,
            Cognome = g.Cognome,
            Telefono = g.Telefono,
            Sesso = g.Sesso
          };
          var resp = await ClientHolder.Client.GetAll<Genitore>().InsertAsync(nuovo);
          genitoreAggiornato = resp.Models.First();
        }

        // Inserisci nuova relazione GenitoreBambino
        var rel = new GenitoreBambino
        {
          IdGenitore = genitoreAggiornato.Id,
          IdBambino = idBambino
        };
        await ClientHolder.Client.GetAll<GenitoreBambino>().InsertAsync(rel);
      }
    }

    // --- Aggiorna iscrizione
    var iscrList = await ClientHolder.Client
        .GetAll<Iscrizione>()
        .Select("*")
        .Where(x => x.IdBambino == idBambino)
        .Get();

    var iscrizione = iscrList.Models?.FirstOrDefault();
    if (iscrizione == null) throw new Exception("Iscrizione non trovata");

    if (dto.AnnoScolastico.HasValue) iscrizione.Anno = (AnnoScolastico?)dto.AnnoScolastico.Value;
    if (dto.Note != null) iscrizione.Note = dto.Note;
    iscrizione.DaIscrivereAlNoi = dto.DaIscrivereAlNoi;
    iscrizione.ModalitaPagamento = dto.ModalitaPagamentoSelezionata;
    iscrizione.FormatoIscrizione = dto.FormatoIscrizioneSelezionato;
    iscrizione.ScontoFratelli = dto.ScontoFratelli;

    await ClientHolder.Client.GetAll<Iscrizione>().Update(iscrizione);

    // --- Aggiorna settimane
    if (dto.Settimane != null)
    {
      // cancella quelle esistenti
      var settimaneExist = await ClientHolder.Client
          .GetAll<IscrizioneSettimana>()
          .Where(x => x.IdIscrizione == iscrizione.Id)
          .Get();
      foreach (var s in settimaneExist.Models ?? new List<IscrizioneSettimana>())
        await ClientHolder.Client.GetAll<IscrizioneSettimana>().Delete(s);

      // inserisci quelle nuove
      foreach (var idSettimana in dto.Settimane)
      {
        await ClientHolder.Client.GetAll<IscrizioneSettimana>().InsertAsync(new IscrizioneSettimana
        {
          IdIscrizione = iscrizione.Id,
          IdSettimana = idSettimana.CostoIntero != null ? idSettimana.Id : -idSettimana.Id,
          Intero = idSettimana.CostoIntero != null
        });
      }
    }

    // --- Aggiorna taglia
    if (dto.Taglia != null)
    {
      var tagliaExist = await ClientHolder.Client
          .GetAll<IscrizioneTaglia>()
          .Where(x => x.IdIscrizione == iscrizione.Id)
          .Get();

      foreach (var t in tagliaExist.Models ?? new List<IscrizioneTaglia>())
        await ClientHolder.Client.GetAll<IscrizioneTaglia>().Delete(t);

      await ClientHolder.Client.GetAll<IscrizioneTaglia>().InsertAsync(new IscrizioneTaglia
      {
        IdIscrizione = iscrizione.Id,
        IdTaglia = dto.Taglia.Value
      });
    }

    // --- Aggiorna scheda sanitaria
    if (dto.AllergieIntolleranze != null || dto.PatologieTerapie != null)
    {
      var schedaExist = await ClientHolder.Client
          .GetAll<SchedaSanitaria>()
          .Where(x => x.IdIscrizione == iscrizione.Id)
          .Get();

      var scheda = schedaExist.Models?.FirstOrDefault();
      if (scheda == null)
      {
        scheda = new SchedaSanitaria { IdIscrizione = iscrizione.Id };
        await ClientHolder.Client.GetAll<SchedaSanitaria>().InsertAsync(scheda);
      }

      if (dto.AllergieIntolleranze != null) scheda.AllergieIntolleranze = dto.AllergieIntolleranze;
      if (dto.PatologieTerapie != null) scheda.PatologieTerapie = dto.PatologieTerapie;

      await ClientHolder.Client.GetAll<SchedaSanitaria>().Update(scheda);
    }

    // --- Aggiorna consensi
    if (dto.ConsensiDisponibili != null)
    {
      var consensiExist = await ClientHolder.Client
          .GetAll<Consenso>()
          .Where(x => x.IdIscrizione == iscrizione.Id)
          .Get();

      foreach (var c in consensiExist.Models ?? new List<Consenso>())
        await ClientHolder.Client.GetAll<Consenso>().Delete(c);

      foreach (var c in dto.ConsensiDisponibili)
      {
        await ClientHolder.Client.GetAll<Consenso>().InsertAsync(new Consenso
        {
          IdIscrizione = iscrizione.Id,
          IdTipoConsenso = c.IdTipoConsenso.Value,
          Valore = c.IsSelected,
        });
      }
    }

    // --- Aggiorna squadre
    //if (dto.IdSquadre != null)
    //{
    //  var squadreExist = await ClientHolder.Client
    //      .GetAll<SquadraBambino>()
    //      .Where(x => x.IdBambino == idBambino)
    //      .Get();

    //  foreach (var s in squadreExist.Models ?? new List<SquadraBambino>())
    //    await ClientHolder.Client.GetAll<SquadraBambino>().Delete(s);

    //  foreach (var idSquadra in dto.IdSquadre)
    //  {
    //    await ClientHolder.Client.GetAll<SquadraBambino>().InsertAsync(new SquadraBambino
    //    {
    //      IdBambino = idBambino,
    //      IdSquadra = idSquadra
    //    });
    //  }
    //}

    return true;
  }

  public static async Task<ModuloIscrizioneDto> GetAsync(int idBambino)
  {
    // --- Bambino
    var bambinoList = await ClientHolder.Client
        .GetAll<Bambino>()
        .Select("*")
        .Where(x => x.Id == idBambino)
        .Get();

    var bambino = bambinoList.Models.FirstOrDefault();
    if (bambino == null)
      throw new Exception();

    // --- Iscrizione
    var iscrList = await ClientHolder.Client
        .GetAll<Iscrizione>()
        .Select("*")
        .Where(x => x.IdBambino == idBambino)
        .Get();

    var iscrizione = iscrList.Models.FirstOrDefault();
    if (iscrizione == null)
      throw new Exception();

    // --- Genitori
    var relGenitoriList = await ClientHolder.Client
        .GetAll<GenitoreBambino>()
        .Select("*")
        .Where(x => x.IdBambino == idBambino)
        .Get();

    var genitoriDto = new List<GenitoreDto>();
    foreach (var rel in relGenitoriList.Models)
    {
      var gen = await ClientHolder.Client
          .GetAll<Genitore>()
          .Select("*")
          .Where(x => x.Id == rel.IdGenitore)
          .Single();

      if (gen != null)
      {
        genitoriDto.Add(new GenitoreDto
        {
          IdGenitore = gen.Id,
          Nome = gen.Nome,
          Cognome = gen.Cognome,
          Telefono = gen.Telefono,
          Sesso = gen.Sesso,
          SessoSelezionato = new SessoItem
          (gen.Sesso == 1 ? "M" : "F", gen.Sesso)
           
        });
      }
    }

    // --- Settimane
    var settimaneList = await ClientHolder.Client
        .GetAll<IscrizioneSettimana>()
        .Select("*")
        .Where(x => x.IdIscrizione == iscrizione.Id)
        .Get();

    var ids = settimaneList.Models
    .Select(s => s.IdSettimana)
    .ToList();

    var settimane = await ClientHolder.Client
        .GetAll<Settimana>()
        .Filter("id_settimana", Supabase.Postgrest.Constants.Operator.In, ids)
        .Get();

    // --- Taglia
    var tagliaList = await ClientHolder.Client
        .GetAll<IscrizioneTaglia>()
        .Select("*")
        .Where(x => x.IdIscrizione == iscrizione.Id)
        .Get();

    var idTaglia = tagliaList.Models.FirstOrDefault()?.IdTaglia;

    // --- Scheda sanitaria
    var schedaList = await ClientHolder.Client
        .GetAll<SchedaSanitaria>()
        .Select("*")
        .Where(x => x.IdIscrizione == iscrizione.Id)
        .Get();

    var scheda = schedaList.Models.FirstOrDefault();

    // --- Consensi
    var consensiList = await ClientHolder.Client
        .GetAll<Consenso>()
        .Select("*")
        .Where(x => x.IdIscrizione == iscrizione.Id)
        .Get();

    var consensiDto = consensiList.Models.Select(c => new ConsensoDto
    {
      IdTipoConsenso = c.IdTipoConsenso,
      IsSelected = c.Valore,
    }).ToList();

    // --- Squadre
    //var squadreList = await ClientHolder.Client
    //    .GetAll<SquadraBambino>()
    //    .Select("*")
    //    .Where(x => x.IdBambino == idBambino)
    //    .Get();

    //var idSquadre = squadreList.Models.Select(s => s.IdSquadra).ToList();

    // --- Costruisci DTO finale
    var output = new ModuloIscrizioneDto
    {
      IdBambino = bambino.Id,
      Nome = bambino.Nome,
      Cognome = bambino.Cognome,
      DataNascita = DateTime.Parse(bambino.DataNascita),
      Genere = bambino.Genere,
      LuogoNascita = bambino.LuogoNascita,
      IndirizzoResidenza = bambino.IndirizzoResidenza,
      IdIscrizione = iscrizione.Id,
      AnnoScolastico = (int?)iscrizione.Anno,
      Note = iscrizione.Note,
      Genitori = genitoriDto,
      Settimane = settimane.Models,
      Taglia = idTaglia,
      AllergieIntolleranze = scheda?.AllergieIntolleranze,
      PatologieTerapie = scheda?.PatologieTerapie,
      ConsensiDisponibili = consensiDto,
      DaIscrivereAlNoi = iscrizione.DaIscrivereAlNoi,
      ModalitaPagamentoSelezionata = iscrizione.ModalitaPagamento,
      FormatoIscrizioneSelezionato = iscrizione.FormatoIscrizione,
      ScontoFratelli = iscrizione.ScontoFratelli
    };

    return output;
  }
}
