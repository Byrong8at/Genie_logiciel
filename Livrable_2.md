# 📝 Cahier des charges - EasySave

## 🎯 Objectifs

### 🔹 Version 1.1 (basée sur la 1.0)
- Ajout de la **possibilité de choisir le format du fichier log** : `JSON` ou `XML`
- Aucune autre évolution fonctionnelle
- Doit être **livrée en même temps** que la version 2.0

### 🔹 Version 2.0 (nouvelle génération)
- Passage à une **interface graphique** (WPF, Avalonia...)
- **Nombre illimité** de travaux de sauvegarde
- **Cryptage des fichiers** via le logiciel **CryptoSoft**
  - Seuls les fichiers avec extensions définies dans les paramètres seront cryptés
- **Fichier log enrichi** avec le **temps de cryptage** :
  - `0` : pas de cryptage
  - `> 0` : durée en millisecondes
  - `< 0` : code erreur
- **Détection d’un logiciel métier** :
  - Si détecté, blocage des sauvegardes
  - Si une sauvegarde séquentielle est en cours, elle termine le fichier en cours avant l’arrêt
  - L’arrêt est **consigné dans le log**
  - Logiciel métier configurable (ex : Calculatrice pour test)
- **Format du fichier log personnalisable** : `JSON` ou `XML`

## ⚠️ Fonctionnalités exclues de la version 2.0
- **Interface par tâche (Play, Pause, Stop)** : prévue pour la **version 3.0**, **non incluse** en 2.0

---

## 📋 Tableau comparatif des versions

| Fonctionnalité                                 | Version 1.0       | Version 1.1          | Version 2.0           |
|-----------------------------------------------|-------------------|----------------------|------------------------|
| **Interface**                                  | Console           | Console              | **Graphique**          |
| **Langues**                                     | FR / EN           | FR / EN              | FR / EN                |
| **Nb de sauvegardes**                           | 5 max             | 5 max                | **Illimité**           |
| **Fichier log**                                 | Oui (JSON)        | **JSON ou XML**      | **JSON ou XML + durée** |
| **Utilisation de DLL pour log**                | Oui               | Oui                  | Oui                    |
| **Fichier état**                                | Oui               | Oui                  | Oui                    |
| **Types de sauvegardes**                        | Mono/Séquentielle | Mono/Séquentielle    | Mono/Séquentielle      |
| **Détection logiciel métier**                  | Non               | Non                  | **Oui**                |
| **Ligne de commande**                           | Oui               | Oui                  | Oui                    |
| **Utilisation de CryptoSoft**                   | Non               | Non                  | **Oui**                |

---

## 📦 Livrables attendus
- **Version 1.1** (console, log XML/JSON)
- **Version 2.0** (graphique, cryptage, détection logiciel métier, log amélioré)
