# 🚜 Forklift Simulator (Prototype)

Un prototype de simulation de chariot élévateur basé sur la physique, développé sous **Unity 6**.
Ce projet sert de démonstration technique d'une **architecture découplée et évolutive** (Clean Code & SOLID).

![Gameplay Demo](demo.gif)

## 🎯 Objectifs Techniques

Le focus principal de ce projet est la **structure du code** et la gestion d'une **physique complexe** :

* **Architecture FSM (Finite State Machine) :** Gestion du flux de jeu via des états isolés (`Boot`, `Briefing`, `Gameplay`, `Win`) pour garantir la modularité.
* **SOLID & Découplage :** Utilisation de `ScriptableObjects` pour l'injection de dépendances (Pattern Observer pour les Inputs). Le contrôleur est agnostique du périphérique (Clavier/Manette).
* **Physique Avancée :**
    * **Gestion du Centre de Masse (CoM) :** Calcul manuel du CoM pour prévenir le basculement du véhicule lors des charges lourdes.
    * **Interaction Matériaux :** Réglage fin des *Friction Combines* (Minimum/Average) pour simuler le glissement réaliste des fourches métalliques sous les palettes en bois.
    * **Contraintes Dynamiques :** Utilisation de `FixedJoints` avec seuil de rupture (`BreakForce`) pour le transport de charge.

## 🎮 Contenu du Prototype

Le projet intègre une boucle de gameplay complète avec deux missions distinctes :

* **Mission 1 : Conduite de Précision**
    * Parcours pour tester la maniabilité.
    * Gestion de l'inertie et des roues arrière directrices.
* **Mission 2 : Logistique & Manutention**
    * Mécanique de Pick & Drop d'une palette avec poids.
    * Validation de l'objectif via une zone de détection intelligente.

## ⌨️ Contrôles

| Action | Touche |
| :--- | :--- |
| **Mouvement** | Z / S (Avancer / Reculer) |
| **Direction** | Q / D (Braquer) |
| **Fourches** | Flèches Haut / Bas (Monter / Descendre) |
| **Caméra** | Clic Droit Maintenu + Souris |
| **Zoom** | Molette Souris |

## 🛠️ Installation

1.  Cloner le dépôt.
2.  Ouvrir avec **Unity 6** (Version 6000.0 ou supérieur).
3.  Lancer la scène `Simulation` située dans le dossier `Assets/Project/Scenes`.

---
*Développé par Nouha Chebbi*
