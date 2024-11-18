# Improbability Police

## Overview
*Improbability Police* is a simple, proof-of-concept game that was developed for a 10-day itch.io game jam with a post-apocalyptic theme, where it came in 1st place. The game leverages GPT-4 for dynamic content generation and player interaction, thus no two playthroughs are identical. The development process provided valuable learning experiences in GPT-4 integration, proxy servers, and AWS. Ultimately, the AWS and OpenAI API costs were too high to keep a demo active long-term, but video of the gameplay can be found at the link below.

## Gameplay Video
- [Watch the Gameplay Video](https://youtu.be/WctE6Gbzzak)

## Game Description
In *Improbability Police*, players are tasked with building a civilization on a newly colonized planet by making decisions of what to build. The game uses GPT-4 to give updates on how the civilization is going and assess the overall happiness of the civilization, as well as the uniqueness of the player's responses.

![Screenshot](Assets/Images/improbability_police_screenshot_2.png)

## What I Learned
The development of *Improbability Police* provided numerous learning opportunities:
- **GPT-4 Integration:** Understanding how to integrate and utilize GPT-4 for dynamic content generation and player interaction.
- **Proxy Servers:** Setting up a Flask proxy server to securely manage API requests.
- **AWS:** Deploying and managing an AWS EC2 instance to host the Flask application, ensuring scalability and reliability.
- **WebGL and itch.io:** Building and deploying a Unity WebGL game and the challenges associated with games that require access to an external API.
- **tmux and Server Management:** Using tmux for managing long-running processes on an EC2 instance.
- **CORS and Network Configuration:** Configuring CORS and security groups to handle cross-origin requests and ensure network security.
- **Working Within a Deadline:** Managing time effectively to develop and deliver the game within the 10-day timeframe of the game jam.
- **Prompt Engineering:** Crafting effective prompts to elicit desired responses from GPT-4, using both zero shot and one shot prompting, which was crucial for generating the right text and scores for the game.

## Narrative Backstory Video
- [Watch the Narrative Backstory Video](https://www.youtube.com/watch?v=IQmGUNdRSwQ)
