Diagramme de Cycle de Vie – EasySave

Ce diagramme représente le "cycle de vie d’un travail de sauvegarde" dans l'application EasySave. Il illustre les différents états par lesquels une sauvegarde peut passer, de sa création jusqu’à sa terminaison, avec la gestion des erreurs et des reprises.

Description des États

- [Début] : Point d’entrée du processus de sauvegarde.
- Initialisé : Le travail de sauvegarde a été créé et configuré.
- EnAttente : Le travail attend son lancement (planification ou déclenchement manuel).
- EnCours : La sauvegarde est actuellement en exécution.
- Terminé : La sauvegarde a été réalisée avec succès.
- Échec : Une erreur est survenue pendant la sauvegarde.
- Reprendre : Le système tente de reprendre une sauvegarde ayant échoué.

Transitions

- Une fois le travail "initialisé", il passe en "attente de lancement".
- Dès que le lancement est déclenché, il passe à l’état "EnCours".
- Si la sauvegarde réussit, elle passe à l’état "Terminé", puis au "[Fin]" du cycle.
- En cas d’échec, le processus passe à l’état "Échec". Il peut ensuite soit :
  - Se terminer si aucune tentative n’est effectuée.
  - Être repris via une tentative de reprise, repassant à l’état "EnCours".

Objectif

Ce diagramme est essentiel pour modéliser le comportement dynamique du système EasySave, notamment la gestion des erreurs, les reprises automatiques, et le suivi de l'état de chaque sauvegarde.

Ce diagramme a été réalisé pour la version 1.0 de EasySave dans le cadre du livrable 1.
