Ce diagramme d'activité permet de représenter le déclenchement des événements en fonction des états du système. Ce diagramme d'activité représente le déclenchement de l'application, traitement et sauvegarde des travaux et la fermeture de l'application quand le traitement est fini.   

1. Démarrage de l'application :

L'application démarre.
Elle charge les paramètres de configuration (emplacements des fichiers journaux et d'état, etc.).
Elle vérifie la disponibilité des travaux de sauvegarde configurés.

2. Entrée utilisateur :

L'utilisateur fournit une entrée en ligne de commande spécifiant les travaux à exécuter (Ex : 1 - 3, 1 ; 3, ou un numéro de travail unique).
L'application valide ou non l'entrée utilisateur.

3. Boucle de traitement des travaux :

Pour chaque travail spécifié :

L'application récupère les paramètres du travail (nom, source, cible, type).
L'application prépare la sauvegarde et crée des répertoires cible si nécessaire. 
L'application enregistre l'état initial du travail dans le fichier d'état (État : "Actif", nombre de fichiers, taille totale, etc.).
L'application parcourt récursivement le répertoire source.

Pour chaque fichier/dossier :

Une copie du fichier ou du dossier est envoyée vers le répertoire cible.
L'application enregistre des informations de sauvegarde dans le fichier journal (horodatage, nom de sauvegarde, chemins source et cible, taille du fichier, temps de transfert). 
L'application met à jour le fichier d'état avec la progression (nombre de fichiers traités, taille totale transférée, nombre de fichiers restants, taille restante, chemin du fichier en cours de traitement).
À la fin du travail, l'application enregistre l'état final du travail dans le fichier d'état (État : "Terminé" ou "Erreur").

4. Fin de l'application :

L'application termine son exécution après avoir traité tous les travaux spécifiés.

L'objectif de ce diagramme est de permettre la visualisation du fonctionnement de l'application étape par étape et donc montre les décisions prises par l'application avec les questions, c'est simplifié pour être le plus clair possible alors la gestion d'erreur qui va être intégrée à chaque étape et la gestion de la langue n'est pas mentionné. 

Ce diagramme a été réalisé pour la version 1.0 de EasySave dans le cadre du livrable 1.
