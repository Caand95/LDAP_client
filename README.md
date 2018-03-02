# LDAP PROGRAM
Information og guide til at oprette forbindelse mm. blev fundet i en artikel på Internettet.
Vores Ldap program er delt op i 2 dele.
En console del og en gui del, og dette skift kan laves ved at bytte rundt på hvad der er Main og “OldMain”.

# Programmet
Programmet kører igennem C#'s DirectoryServices LdapConnection der bliver gemt bag vores klasse: Connection.
Når applikationen skal modtage informationer om domænet: brugere, grupper, ou'er 
og i det hele taget få indlæst hierarkiet, bruger vi klassen Directory og bruger Ldap-klassen
DirectoryEntry, som jeg finder er bedre til den slags. 
Man skal være opmærksom på at første gang man klikker på et item i tree-viewet,
at det tager lidt tid for den at arbejde/forbinde. Derefter er den klar og det går relativit hurtigt
for den at få de forskellige informationer om de forskellige brugere osv. 
Man kan foretage simple operationer i programmet såsom at se egenskaber af entiteter og
redigere eller slette en bruger.
