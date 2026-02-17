using Supabase.Postgrest.Attributes;
using IscrizioniManager.Data;
using System;


[Table("v_iscrizione_completa")]  // <-- your view name
public class VIscrizioneCompleta : EventModel
{
  [PrimaryKey("id_bambino")]
  [Column("id_bambino")]
  public int IdBambino { get; set; }

  [Column("b_cognome")]
  public string BCognome { get; set; }

  [Column("b_nome")]
  public string BNome { get; set; }

  [Column("data_nascita")]
  public DateTime DataNascita { get; set; }

  [System.ComponentModel.DataAnnotations.Schema.NotMapped]
  public string DataNascitaDesc { get; set; }

  [Column("luogo_nascita")]
  public string LuogoNascita { get; set; }

  [Column("indirizzo_residenza")]
  public string IndirizzoResidenza { get; set; }
  [Column("comune_residenza")]
  public string ComuneResidenza { get; set; }
  [Column("id_genitore")]
  public int? IdGenitore { get; set; }   // nullable because of LEFT JOIN

  [Column("g_cognome")]
  public string GCognome { get; set; }

  [Column("g_nome")]
  public string GNome { get; set; }

  [Column("telefono")]
  public string Telefono { get; set; }

  [Column("id_iscrizione")]
  public int? IdIscrizione { get; set; }

  [Column("anno")]
  public int? Anno { get; set; }

  [System.ComponentModel.DataAnnotations.Schema.NotMapped]
  public string AnnoDesc { get; set; }

  [Column("s_desc")]
  public string SDesc { get; set; }

  [Column("t_codice")]
  public string TCodice { get; set; }

  [Column("c_descrizione")]
  public string CDescrizione { get; set; }

  [Column("c_valore")]
  public string CValore { get; set; }

  [Column("allergie_intolleranze")]
  public string AllergieIntolleranze { get; set; }

  [Column("patologie_terapie")]
  public string PatologieTerapie { get; set; }

  [Column("pagato")]
  public bool Pagato { get; set; }

  [Column("note")]
  public string Note { get; set; }
}
