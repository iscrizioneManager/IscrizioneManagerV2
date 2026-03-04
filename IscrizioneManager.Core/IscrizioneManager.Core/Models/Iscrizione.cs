using IscrizioniManager.Data;
using IscrizioniManager.Models;
using Supabase.Postgrest.Attributes;

[Table("iscrizione")]
public class Iscrizione : EventModel
{
  [PrimaryKey("id_iscrizione")]
  public int Id { get; set; }

  [Column("id_bambino")]
  public int IdBambino { get; set; }

  [Column("anno")]
  public AnnoScolastico? Anno { get; set; }

  [Column("note")]
  public string? Note { get; set; }

  [Column("pagato")]
  public bool Pagato { get; set; }

  [Column("inclusa_iscrizione_noi")]
  public bool DaIscrivereAlNoi { get; set; }

  [Column("modalita_pagamento")]
  public int? ModalitaPagamento { get; set; }

  [Column("formato_iscrizione")]
  public int? FormatoIscrizione { get; set; }
  [Column("sconto_fratelli")]
  public bool ScontoFratelli { get; set; }
  [Column("ricevuta")]
  public bool Ricevuta { get; set; }
    [Column("desc_ricevuta")]
    public string DescRicevuta { get; set; }
    [Column("esce_solo")]
    public bool EsceSolo { get; set; }
}