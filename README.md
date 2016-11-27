# mandrilkalendar
Mandriljulekalender

## TODO

* [X] Skedulering af notificationer
 - [X] Der mangler implementation af INotificationService i .Droid projektet.
 - [X] Skedulering forslår jeg laves i App.xaml.cs.
* [X] Regl for hvornår hver enkelt låge må åbnes.
 - [X] Codepointer: GateControl#OnTapped().
* [X] Implementation af lyd når appen åbner samt når en notifikation vises.
 - [X] lyd filen ligger i root af repo. Denne skal flyttes til de relevante resource mapper i iOS samt Droid projektet.
* [ ] Embbed afspilning af youtube.
 - [ ] Der skal laves platform specifikke bindings.
 - [ ] Dokumentation Android: https://developers.google.com/youtube/android/player/
 - [ ] Dokumentation iOS https://developers.google.com/youtube/v3/guides/ios_youtube_helper
 - [ ] Alternativt kan vi overveje bare at vise det i et webview via en iframe. Dette er dog ikke særlig pænt.
