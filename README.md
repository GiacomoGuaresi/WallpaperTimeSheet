# WallpaperTimeSheet

**WallpaperTimeSheet** è un'applicazione Windows in C# progettata per tracciare automaticamente le ore spese al PC lavorando su diversi task o progetti. Il programma consente di selezionare l'attività attualmente in corso tramite un'icona nella tray di sistema, e genera uno sfondo del desktop che mostra un riepilogo grafico delle ore lavorative, suddivise per task e per giorno nel mese corrente.

## Funzionalità

- **Selezione attività dalla tray**: seleziona il task o progetto a cui stai lavorando direttamente dall'icona nella system tray.
- **Conteggio automatico delle ore**: il programma registra automaticamente il tempo speso sull'attività selezionata.
- **Sfondo dinamico del desktop**: WallpaperTimeSheet genera uno sfondo del desktop con un grafico riepilogativo delle ore spese nel mese corrente, suddiviso per task e per giorno.
  - Il calendario mostra fino a 6 settimane, in modo simile al calendario di Windows, con ogni task colorato in base alla configurazione utente.
  - Riepilogo delle ore spese per ciascun task.
  - Visualizzazione dell'attività attualmente selezionata.
  
## Tecnologie utilizzate

- **C#**
- **SQLite** per il salvataggio dei dati localmente.
- **Windows API** per la gestione della tray icon e del desktop wallpaper.

## Requisiti

- Sistema operativo Windows
- .NET Framework 4.7.2 o successivo

## Istruzioni per l'uso

1. Avviare il programma e selezionare il task attuale dall'icona nella tray.
2. Il programma inizierà automaticamente a conteggiare le ore spese per quell'attività.
3. Lo sfondo del desktop verrà aggiornato in tempo reale con il grafico delle ore spese per ciascun task.
4. È possibile cambiare task in qualsiasi momento selezionandolo nuovamente dalla tray.

## WIP (Work in Progress)

WallpaperTimeSheet è ancora in fase di sviluppo, e le seguenti funzionalità sono in corso di implementazione:

- **Pannello di configurazione**: sarà possibile personalizzare i colori e le impostazioni del programma.
- **Installer**: verrà rilasciato un installer per facilitare l'installazione.
- **Salvataggio delle impostazioni su SQLite**: tutte le informazioni relative alle attività verranno memorizzate in un database locale SQLite.

## Screenshot

![Immagine 2024-12-14 142358](https://github.com/user-attachments/assets/5af3f994-1a36-4b08-8d6f-3402df31142e)

![Immagine 2024-12-14 142257](https://github.com/user-attachments/assets/e9c21f79-40e8-484c-afd8-155f6743df74)

## Licenza

GNU GENERAL PUBLIC LICENSE

