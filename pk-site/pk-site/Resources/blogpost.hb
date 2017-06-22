---
layout: post
title: {{Title}}
date: {{Date}}
excerpt: {{Title}}
categories: ["pokemon"]
tags: ["pokemon", "video games"]
---

### Name: {{Model.TrainerInfo.Name}}

Money: {{Model.TrainerInfo.FormattedMoney}}

Play Time: {{Model.TrainerInfo.PlayTime}}

Pok&eacute;mon Caught: {{Model.TrainerInfo.CaughtCount}}, Seen: {{Model.TrainerInfo.SeenCount}}

#### Party Pok&eacute;mon

<section class="section">
	<div>
		<div class="columns is-multiline is-mobile">
			{{#Model.PartyMembers}}
				{{> party}}
			{{/Model.PartyMembers}}
		</div>
	</div>
</section>

#### Pok&eacute;mon in Boxes  ({{BoxPokemonCount}}!)

<section class="section">
	<div>
		<div class="columns is-multiline is-mobile">
			{{#Model.BoxPokemon}}
				{{> boxpokemon}}
			{{/Model.BoxPokemon}}
		</div>
	</div>
</section>
