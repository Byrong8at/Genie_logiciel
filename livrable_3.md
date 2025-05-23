# 🗂️ Livrable 3 – Synthèse des évolutions EasySave 3.0 & Propositions Version 4.0

## ✅ Évolutions apportées en EasySave 3.0

- **Sauvegardes en parallèle** : abandon du mode séquentiel.
- **Gestion des fichiers prioritaires** :
  - Les extensions prioritaires sont traitées en premier.
- **Blocage des transferts simultanés > N Ko** :
  - Paramétrable pour éviter la saturation réseau.
- **Contrôle utilisateur en temps réel** :
  - Pause (post-transfert), Reprise, Arrêt immédiat.
  - Suivi de l’avancement (pourcentage de progression).
- **Détection automatique des logiciels métiers** :
  - Mise en pause des sauvegardes pendant l’exécution.
- **Console distante (IHM déportée)** :
  - Interface graphique connectée par sockets.
- **CryptoSoft Mono-Instance** :
  - Garantit qu'une seule instance fonctionne à la fois.
- **Option de réduction automatique en cas de charge réseau** :
  - Dynamique, selon un seuil configurable.

---

## 📊 Synthèse des fonctionnalités (comparatif)

| Fonctionnalité                            | Version 3.0              |
|------------------------------------------|--------------------------|
| Interface graphique                      | ✅                        |
| Multi-langues                            | Anglais & Français       |
| Nombre de travaux                        | Illimité                 |
| Fichier log (JSON, XML)                  | ✅                        |
| Fichier état                             | ✅                        |
| Pause/Stop/Play par tâche                | ✅                        |
| Fonctionnement en parallèle              | ✅                        |
| Détection logiciel métier                | ✅ (pause automatique)   |
| Utilisation CryptoSoft                   | ✅ (Mono-instance)       |
| Gestion fichiers prioritaires            | ✅                        |
| Blocage transfert gros fichiers simultanés | ✅                      |
| Console de supervision distante          | ✅                        |
| Surveillance charge réseau               | ✅ (optionnelle)         |

---

## 🚀 Propositions pour EasySave 4.0

| Fonctionnalité proposée                   | Bénéfice utilisateur                | Complexité dev. estimée |
|------------------------------------------|-------------------------------------|--------------------------|
| Sauvegardes planifiées (calendrier)      | Automatisation complète             | 🟨 Moyenne (3-4 sem.)    |
| Intégration Cloud (Google, OneDrive...)  | Sécurité externe, mobilité          | 🟥 Élevée (6-8 sem.)     |
| Sauvegardes différentielles/incrémentales| Gains réseau et espace              | 🟨 Moyenne (4-5 sem.)    |
| Dashboard statistiques                   | Suivi visuel et analyse             | 🟩 Faible (2-3 sem.)     |
| Recherche dans l’historique              | Gain de productivité                | 🟩 Faible (1-2 sem.)     |
| Notifications (mail, Slack, pop-up)      | Réactivité                          | 🟨 Moyenne (2-3 sem.)    |
| Chiffrement natif EasySave               | Autonomie, sécurité renforcée       | 🟥 Élevée (6+ sem.)      |

---

## ⚙️ Analyse du mode de sauvegarde

| Mode            | Avantage                        | Inconvénient                 | Recommandation     |
|-----------------|----------------------------------|-------------------------------|---------------------|
| Séquentiel      | Simplicité                      | Trop lent                    | ❌ Obsolète         |
| Parallèle       | Gain de performance             | Risque de surcharge réseau   | ✅ À conserver      |
| Hybride adaptatif (v4.0) | Optimisé selon charge réseau et priorités | Complexité accrue         | 🟨 À explorer       |

---

## 🎯 Conclusion & Recommandations

- **Maintenir les sauvegardes parallèles**, mais régulées (fichiers prioritaires, gros fichiers).
- Viser une **version 4.0 orientée Cloud, automatisation, adaptabilité réseau**.
- Objectif : faire d’EasySave une solution de sauvegarde **autonome, connectée et intelligente**.

