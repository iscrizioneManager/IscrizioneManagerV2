using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IscrizioneManager.Core.Repositories;

public class SupabaseIscrizioneCompletaRepository : IIscrizioneCompletaRepository
{
    public async Task<bool> CreateAsync(ModuloIscrizioneDto dto)
    {
        var payload = new
        {
            p_dto = new
            {
                nome = dto.Nome,
                cognome = dto.Cognome,
                data_nascita = dto.DataNascita?.ToString("yyyy-MM-dd"),
                genere = dto.Genere,
                luogo_nascita = dto.LuogoNascita,
                indirizzo_residenza = dto.IndirizzoResidenza,
                comune_residenza = dto.ComuneResidenza,
                anno_scolastico = dto.AnnoScolastico,
                note = dto.Note,
                da_iscrivere_al_noi = dto.DaIscrivereAlNoi,
                altra_parrocchia = !dto.DaIscrivereAlNoi ? dto.AltraParrocchia : false,
                formato_iscrizione = dto.FormatoIscrizioneSelezionato,
                modalita_pagamento = dto.ModalitaPagamentoSelezionata,
                sconto_fratelli = dto.ScontoFratelli,
                ricevuta = dto.Ricevuta,
                desc_ricevuta = dto.Ricevuta ? dto.DescRicevuta : null,
                esce_solo = dto.EsceSolo,
                caparra_pagata = dto.CaparraPagata,
                email_genitore = dto.EmailGenitore,
                priority = dto.Priority,
                settimane = dto.Settimane?.Where(x => x.IsSelected).Select(s => new {
                    id_settimana = Math.Abs(s.Id),
                    intero = s.CostoIntero != null
                }).ToArray(),
                genitori = dto.Genitori?.Select(g => new {
                    id_genitore = g.IdGenitore,
                    nome = g.Nome,
                    cognome = g.Cognome,
                    telefono = g.Telefono,
                    sesso = g.Genere
                }).ToArray(),
                consensi = dto.ConsensiDisponibili?.Select(c => new {
                    id_tipo_consenso = c.IdTipoConsenso,
                    valore = c.IsSelected
                }).ToArray(),
                Taglia = dto.Taglia,
                allergie_intolleranze = dto.AllergieIntolleranze,
                patologie_terapie = dto.PatologieTerapie,
                event_id = ClientHolder.Client._eventId
            }
        };

        try
        {
            var bambinoId = await ClientHolder.Client.Rpc<int>("create_iscrizione", payload);
            return bambinoId != default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore CreateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ModuloIscrizioneDto dto)
    {
        var payload = new
        {
            p_dto = new
            {
                nome = dto.Nome,
                cognome = dto.Cognome,
                data_nascita = dto.DataNascita?.ToString("yyyy-MM-dd"),
                genere = dto.Genere,
                luogo_nascita = dto.LuogoNascita,
                indirizzo_residenza = dto.IndirizzoResidenza,
                comune_residenza = dto.ComuneResidenza,
                anno_scolastico = dto.AnnoScolastico,
                note = dto.Note,
                da_iscrivere_al_noi = dto.DaIscrivereAlNoi,
                altra_parrocchia = !dto.DaIscrivereAlNoi ? dto.AltraParrocchia : false,
                formato_iscrizione = dto.FormatoIscrizioneSelezionato,
                modalita_pagamento = dto.ModalitaPagamentoSelezionata,
                sconto_fratelli = dto.ScontoFratelli,
                ricevuta = dto.Ricevuta,
                desc_ricevuta = dto.Ricevuta ? dto.DescRicevuta : null,
                esce_solo = dto.EsceSolo,
                caparra_pagata = dto.CaparraPagata,
                email_genitore = dto.EmailGenitore,
                priority = dto.Priority,
                settimane = dto.Settimane?.Where(x => x.IsSelected).Select(s => new {
                    id_settimana = Math.Abs(s.Id),
                    intero = s.CostoIntero != null
                }).ToArray(),
                genitori = dto.Genitori?.Select(g => new {
                    id_genitore = g.IdGenitore,
                    nome = g.Nome,
                    cognome = g.Cognome,
                    telefono = g.Telefono,
                    sesso = g.Genere
                }).ToArray(),
                consensi = dto.ConsensiDisponibili?.Select(c => new {
                    id_tipo_consenso = c.IdTipoConsenso,
                    valore = c.IsSelected
                }).ToArray(),
                taglia = dto.Taglia,
                allergie_intolleranze = dto.AllergieIntolleranze,
                patologie_terapie = dto.PatologieTerapie,
                event_id = ClientHolder.Client._eventId
            },
            p_id_bambino = dto.IdBambino
        };

        try
        {
            var bambinoId = await ClientHolder.Client.Rpc<int>("update_iscrizione", payload);
            return bambinoId != default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int idBambino)
    {
        var payload = new { p_id_bambino = idBambino };
        try
        {
            var deletedId = await ClientHolder.Client.Rpc<int>("delete_iscrizione", payload);
            return deletedId != default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore DeleteAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<ModuloIscrizioneDto> GetAsync(int idBambino)
    {
        var bambinoList = await ClientHolder.Client
            .GetAll<Bambino>()
            .Select("*")
            .Where(x => x.Id == idBambino)
            .Get();

        var bambino = bambinoList.Models.FirstOrDefault();
        if (bambino == null) throw new Exception();

        var iscrList = await ClientHolder.Client
            .GetAll<Iscrizione>()
            .Select("*")
            .Where(x => x.IdBambino == idBambino)
            .Get();

        var iscrizione = iscrList.Models.FirstOrDefault();
        if (iscrizione == null) throw new Exception();

        var relGenitoriList = await ClientHolder.Client
            .GetAll<GenitoreBambino>()
            .Select("*")
            .Where(x => x.IdBambino == idBambino)
            .Get();

        var genitoriDto = new List<GenitoreDto>();
        foreach (var rel in relGenitoriList.Models)
        {
            var gen = await ClientHolder.Client
                .BaseFrom<Genitore>()
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

        var settimaneList = await ClientHolder.Client
            .GetAll<IscrizioneSettimana>()
            .Select("*")
            .Where(x => x.IdIscrizione == iscrizione.Id)
            .Get();

        var ids = settimaneList.Models.Select(s => s.IdSettimana).ToList();

        var settimane = await ClientHolder.Client
            .GetAll<Settimana>()
            .Filter("id_settimana", Supabase.Postgrest.Constants.Operator.In, ids)
            .Get();

        var tagliaList = await ClientHolder.Client
            .GetAll<IscrizioneTaglia>()
            .Select("*")
            .Where(x => x.IdIscrizione == iscrizione.Id)
            .Get();

        var idTaglia = tagliaList.Models.FirstOrDefault()?.IdTaglia;

        var schedaList = await ClientHolder.Client
            .GetAll<SchedaSanitaria>()
            .Select("*")
            .Where(x => x.IdIscrizione == iscrizione.Id)
            .Get();

        var scheda = schedaList.Models.FirstOrDefault();

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

        var tutteSettimane = await ClientHolder.Client
            .GetAll<Settimana>()
            .Select("*")
            .Get();

        var result = new List<Settimana>();
        foreach (var t in settimane.Models)
        {
            if (t.CostoIntero != null)
            {
                result.Add(new Settimana
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
                result.Add(new Settimana
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

        return new ModuloIscrizioneDto
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
            AnnoScolastico = iscrizione.Anno,
            Note = iscrizione.Note,
            Genitori = genitoriDto,
            Settimane = result,
            Taglia = idTaglia,
            AllergieIntolleranze = scheda?.AllergieIntolleranze,
            PatologieTerapie = scheda?.PatologieTerapie,
            ConsensiDisponibili = consensiDto,
            DaIscrivereAlNoi = iscrizione.DaIscrivereAlNoi,
            AltraParrocchia = iscrizione.AltraParrocchia,
            ModalitaPagamentoSelezionata = iscrizione.ModalitaPagamento,
            FormatoIscrizioneSelezionato = iscrizione.FormatoIscrizione,
            ScontoFratelli = iscrizione.ScontoFratelli,
            Ricevuta = iscrizione.Ricevuta,
            DescRicevuta = iscrizione.DescRicevuta,
            EsceSolo = iscrizione.EsceSolo,
            CaparraPagata = iscrizione.CaparraPagata,
            EmailGenitore = iscrizione.EmailGenitore,
            Priority = iscrizione.Priority ?? false
        };
    }
}
