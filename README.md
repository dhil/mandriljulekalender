# mandrilkalendar
Mandriljulekalender

## TODO
1. Skedulering af notificationer
 * Der mangler implementation af INotificationService i .Droid projektet.
 * Skedulering forslår jeg laves i App.xaml.cs.
2. Regl for hvornår hver enkelt låge må åbnes.
 * Codepointer: GateControl#OnTapped().
3. Implementation af lyd når appen åbner samt når en notifikation vises.
 * lyd filen ligger i root af repo. Denne skal flyttes til de relevante resource mapper i iOS samt Droid projektet.
4. Embbed afspilning af youtube.
 * Der skal laves platform specifikke bindings.
 * Dokumentation Android: https://developers.google.com/youtube/android/player/
 * Dokumentation iOS https://developers.google.com/youtube/v3/guides/ios_youtube_helper
 * Alternativt kan vi overveje bare at vise det i et webview via en iframe. Dette er dog ikke særlig pænt.
