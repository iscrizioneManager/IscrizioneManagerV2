using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

public class IscrizioneCompletaController
{
  public IscrizioneCompletaController()
  {
  }

  public static async Task<bool> CreateAsync(ModuloIscrizioneDto dto)
  {
    if (dto == null) throw new ArgumentNullException(nameof(dto));
        var payload = new
        {
            p_dto = new
            {
                // Bambino
                nome = dto.Nome,
                cognome = dto.Cognome,
                data_nascita = dto.DataNascita?.ToString("yyyy-MM-dd"),
                genere = dto.Genere,
                luogo_nascita = dto.LuogoNascita,
                indirizzo_residenza = dto.IndirizzoResidenza,
                comune_residenza = dto.ComuneResidenza,
                anno_scolastico = dto.AnnoScolastico,
                note = dto.Note,

                // Iscrizione
                da_iscrivere_al_noi = dto.DaIscrivereAlNoi,
                formato_iscrizione = dto.FormatoIscrizioneSelezionato,
                modalita_pagamento = dto.ModalitaPagamentoSelezionata,
                sconto_fratelli = dto.ScontoFratelli,
                ricevuta = dto.Ricevuta,
                desc_ricevuta = dto.Ricevuta ? dto.DescRicevuta : null,
                esce_solo = dto.EsceSolo,

                // Settimane
                settimane = dto.Settimane?.Where(x => x.IsSelected).Select(s => new {
                    id_settimana = Math.Abs(s.Id),
                    intero = s.CostoIntero != null
                }).ToArray(),

                // Genitori
                genitori = dto.Genitori?.Select(g => new {
                    id_genitore = g.IdGenitore,
                    nome = g.Nome,
                    cognome = g.Cognome,
                    telefono = g.Telefono,
                    sesso = g.Genere
                }).ToArray(),

                // Consensi
                consensi = dto.ConsensiDisponibili?.Select(c => new {
                    id_tipo_consenso = c.IdTipoConsenso,
                    valore = c.IsSelected
                }).ToArray(),

                // Taglia
                Taglia = dto.Taglia,  // maiuscolo perché la funzione controlla 'Taglia'

                // Scheda sanitaria
                allergie_intolleranze = dto.AllergieIntolleranze,
                patologie_terapie = dto.PatologieTerapie,
                event_id = ClientHolder.Client._eventId
            }
        };


        try
        {
      // 1. Serializza il DTO in JSON
      var dtoJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });

      // 2. Chiama la stored procedure PostgreSQL via RPC
      //    "create_iscrizione" è il nome della funzione PL/pgSQL
      var bambinoId = await ClientHolder.Client.Rpc<int>(
          "create_iscrizione",
          payload  // pass as JSON, not string
      );

      // 3. Restituisci l'id del bambino creato
      return bambinoId != default;
    }
    catch (Exception ex)
    {
      // Qui puoi loggare o rilanciare l'eccezione
      Console.WriteLine($"Errore CreateAsync: {ex.Message}");
      throw;
    }
  }

  public static async Task<bool> UpdateAsync(ModuloIscrizioneDto dto)
  {
    if (dto == null) throw new ArgumentNullException(nameof(dto));
        var payload = new
        {
            p_dto = new
            {
                // Bambino
                nome = dto.Nome,
                cognome = dto.Cognome,
                data_nascita = dto.DataNascita?.ToString("yyyy-MM-dd"),
                genere = dto.Genere,
                luogo_nascita = dto.LuogoNascita,
                indirizzo_residenza = dto.IndirizzoResidenza,
                comune_residenza = dto.ComuneResidenza,

                // Iscrizione
                anno_scolastico = dto.AnnoScolastico,
                note = dto.Note,
                da_iscrivere_al_noi = dto.DaIscrivereAlNoi,
                formato_iscrizione = dto.FormatoIscrizioneSelezionato,
                modalita_pagamento = dto.ModalitaPagamentoSelezionata,
                sconto_fratelli = dto.ScontoFratelli,
                ricevuta = dto.Ricevuta,
                desc_ricevuta = dto.Ricevuta ? dto.DescRicevuta : null,
                esce_solo = dto.EsceSolo,

                // Settimane
                settimane = dto.Settimane?.Where(x => x.IsSelected).Select(s => new {
                    id_settimana = Math.Abs(s.Id),
                    intero = s.CostoIntero != null
                }).ToArray(),

                // Genitori
                genitori = dto.Genitori?.Select(g => new {
                    id_genitore = g.IdGenitore,
                    nome = g.Nome,
                    cognome = g.Cognome,
                    telefono = g.Telefono,
                    sesso = g.Genere
                }).ToArray(),

                // Consensi
                consensi = dto.ConsensiDisponibili?.Select(c => new {
                    id_tipo_consenso = c.IdTipoConsenso,
                    valore = c.IsSelected
                }).ToArray(),

                // Taglia
                taglia = dto.Taglia,

                // Scheda sanitaria
                allergie_intolleranze = dto.AllergieIntolleranze,
                patologie_terapie = dto.PatologieTerapie,
                event_id = ClientHolder.Client._eventId
            },
            p_id_bambino = dto.IdBambino
        };

        try
    {
      // 1. Serializza il DTO in JSON
      var dtoJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });

      // 2. Chiama la stored procedure PostgreSQL via RPC
      //    "create_iscrizione" è il nome della funzione PL/pgSQL
      var bambinoId = await ClientHolder.Client.Rpc<int>(
          "update_iscrizione",
          payload  // pass as JSON, not string
      );

      // 3. Restituisci l'id del bambino creato
      return bambinoId != default;
    }
    catch (Exception ex)
    {
      // Qui puoi loggare o rilanciare l'eccezione
      Console.WriteLine($"Errore CreateAsync: {ex.Message}");
      throw;
    }
  }

  public static async Task<bool> DeleteAsync(int idBambino)
  {
    if (idBambino <= 0)
      throw new ArgumentException("Id non valido", nameof(idBambino));

    var payload = new
    {
      p_id_bambino = idBambino
    };

    try
    {
      var deletedIscrizioneId = await ClientHolder.Client.Rpc<int>(
        "delete_iscrizione",
        payload
      );

      return deletedIscrizioneId != default;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Errore DeleteAsync: {ex.Message}");
      throw;
    }
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
          Genere = gen.Genere
           
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

    var tipiConsensiList = await ClientHolder.Client
        .GetAll<TipoConsenso>()
        .Select("*")
        .Get();

    var consensiDto = consensiList.Models.Select(c => new ConsensoDto
    {
      IdTipoConsenso = c.IdTipoConsenso,
      IsSelected = c.Valore,
      Descrizione = tipiConsensiList.Models.FirstOrDefault(t => t.Id == c.IdTipoConsenso)?.Descrizione
    }).ToList();

        // --- Squadre
        //var squadreList = await ClientHolder.Client
        //    .GetAll<SquadraBambino>()
        //    .Select("*")
        //    .Where(x => x.IdBambino == idBambino)
        //    .Get();

        //var idSquadre = squadreList.Models.Select(s => s.IdSquadra).ToList();
        var tutteSettimane = await ClientHolder.Client
     .GetAll<Settimana>()
     .Select("*")
     .Get();

        var result = new List<Settimana>();
        foreach (var t in settimane.Models)
        {
            if (t.CostoIntero != null)
            {
                result.Add(new Settimana()
                {
                    Id = t.Id,
                    Desc = $"{t.Desc} (Con pranzo)",
                    CostoIntero = t.CostoIntero,
                    CostoBase = null,
                    DataInizio = t.DataInizio,
                    DataFine = t.DataFine,
                    IsSelected = settimaneList.Models.Any(x => x.Intero && x.IdSettimana == t.Id)
                });
            }
            if (t.CostoBase != null)
            {
                result.Add(new Settimana()
                {
                    Id = -t.Id,
                    Desc = $"{t.Desc} (Senza pranzo)",
                    CostoIntero = null,
                    CostoBase = t.CostoBase,
                    DataInizio = t.DataInizio,
                    DataFine = t.DataFine,
                    IsSelected = settimaneList.Models.Any(x => !x.Intero && x.IdSettimana == t.Id)
                });
            }
        }
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
      ComuneResidenza = bambino.ComuneResidenza,
      IdIscrizione = iscrizione.Id,
      AnnoScolastico = (int?)iscrizione.Anno,
      Note = iscrizione.Note,
      Genitori = genitoriDto,
      Settimane = result,
      Taglia = idTaglia,
      AllergieIntolleranze = scheda?.AllergieIntolleranze,
      PatologieTerapie = scheda?.PatologieTerapie,
      ConsensiDisponibili = consensiDto,
      DaIscrivereAlNoi = iscrizione.DaIscrivereAlNoi,
      ModalitaPagamentoSelezionata = iscrizione.ModalitaPagamento,
      FormatoIscrizioneSelezionato = iscrizione.FormatoIscrizione,
      ScontoFratelli = iscrizione.ScontoFratelli,
      Ricevuta = iscrizione.Ricevuta,
      DescRicevuta = iscrizione.DescRicevuta,
      EsceSolo = iscrizione.EsceSolo
    };

    return output;
  }
}
