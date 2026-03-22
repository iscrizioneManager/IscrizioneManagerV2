using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

namespace IscrizioneManager.Core.Models
{
  [Table("evento_metadata")]
  public class EventoMetadata : EventModel
  {
    [PrimaryKey("id")]
    public int Id { get; set; }
    [PrimaryKey("view_genitori")]
    public bool View_Genitori { get; set; }
    [PrimaryKey("view_mail_genitore")]
    public bool View_MailGenitore { get; set; }
    [PrimaryKey("view_anno_scolastico")]
    public bool View_AnnoScolastico { get; set; }
    [PrimaryKey("view_settimane")]
    public bool View_Settimane { get; set; }
    [PrimaryKey("view_scheda_sanitaria")]
    public bool View_SchedaSanitaria { get; set; }
    [PrimaryKey("view_consensi")]
    public bool View_Consensi { get; set; }
    [PrimaryKey("view_formato_iscr_pagamento")]
    public bool View_FormatoIscrPagamento { get; set; }
    [PrimaryKey("view_iscrizione_noi")]
    public bool View_IscrizioneNoi { get; set; }
    [PrimaryKey("view_sconto_fratelli")]
    public bool View_ScontoFratelli { get; set; }
    [PrimaryKey("view_taglia")]
    public bool View_Taglia { get; set; }
    [PrimaryKey("view_uscita_autonoma")]
    public bool View_UscitaAutonoma { get; set; }
  }
}
