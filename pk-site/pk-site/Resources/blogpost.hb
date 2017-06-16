---
layout: pokemonpost
title: {{Title}}
date: {{Date}}
categories: ["pokemon"]
tags: ["pokemon", "video games"]
---

### Name: {{Model.TrainerInfo.Name}}

Money: {{Model.TrainerInfo.FormattedMoney}}

Play Time: {{Model.TrainerInfo.PlayTime}}

Pokemon Caught: {{Model.TrainerInfo.CaughtCount}}, Seen: {{Model.TrainerInfo.SeenCount}}

#### Party Pokemon

<section class="section">
	<div>
		<div class="columns is-multiline is-mobile">
			{{#Model.PartyMembers}}
				{{> party}}
			{{/Model.PartyMembers}}
		</div>
	</div>
</section>

#### Pokemon in Boxes  ({{BoxPokemonCount}}!)

<section class="section">
	<div>
		<div class="columns is-multiline is-mobile">
			{{#Model.BoxPokemon}}
				{{> boxpokemon}}
			{{/Model.BoxPokemon}}
		</div>
	</div>
</section>