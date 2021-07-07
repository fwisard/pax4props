![](./colorlogo.png)


# pax4props 

## Vision and goal

pax4props' goal is to provide a simple passenger simulation to the unpressurized propeller aircrafts (piston and turboprops).
The few existing PAX simulators out there focus on larger planes, with flight attendants and onboard entertainment, while
pax4props will focus on the General Aviation and small commuters aircrafts.

pax4props will track your virtual passengers' stress levels and rank your flight according to that. Hear them complain when they don't like your flying, hear them scream when they panic.

For those who might not know, "PAX" is aviation jargon
and means "passengers".

### The core principles

1. **No money.** No economic simulation. You won't earn nor spend nor even hear about money in pax4props. And of course, no real life money involved, not even donations.
2. **No locked content.** No career progression. You can fly whatever suitable plane you own, no matter your skills, training, or lack thereof.
3. **No mandatory missions/framing.** Compatibility with virtual airlines, online ATC, role-play scenarios, bush trips... It also means the passengers won't be detailed too much.
Ideally, the voices should be neutral/compatible with all ages, cultures and genders. I won't assume much about your PAX.
4. **Simplicity.** No complex setup. No tedious steps. Ideally, just hop on the plane and fly. Lightweight UI.
5. **Low FPS impact.** 
6. **No internet required.** There's an "automatically check for updates" for your convenience but you can disable it in the settings and you
don't have to use it. There's no spying code, no tracking. I don't want your data. Also, obviously, no ads.
7. **Wholesome.** Let's keep it appropriate for all kind of audiences. Usage of profanity, especially in the context of a potential air disaster, is often understandable
but let's keep it family-friendly. I don't want this software to be blocked anywhere because of word sensitivity. For the same reason, maybe it's best we keep religion out
of it.


### The passengers' comfort

Everybody (in the real world) reacts differently based on their psychology and physiology. pax4props' passengers on
the other have much less variance in their needs. They're neither very bold nor very strong. Take good care of them.

1. **Takeoffs and landing are stressful**. Always. Your PAX want the shortest amount of time when flying close to the ground.
2. **Taxiing fast is scary and uncomfortable**. And that includes take-off and landing rolls. 
3. **High climb rates are uncomfortable**. Ears and stomach feel weird.
4. **High descent rates are uncomfortable AND scary**.
5. **Steep turns are uncomfortable** and scary. Watch your bank angle.
6. **Noise is uncomfortable**. It's hard to simulate noise accurately in MSFS, so your PAX' sensitivity may vary from plane to plane,
but the key thing is to not let the propellers turn too fast. And no, they don't have hearing protection devices (you're the only one with a headset).
7. **Hypoxemia is scary**. Not incomfortable per se, but your PAX won't tolerate high altitudes, especially if they're on a regular flight 
and not on a bush trip. Your aircraft is unpressurized and you have no supplemental oxygen for your PAX. Stay under 8000 ft AMSL for maximum safety and comfort. 

8. **Turbulence is uncomfortable**. PAX want smooth flights. Be gentle on the yoke/stick and try not to fly into turbulence.
9. **Sightseeing is soothing**. Due to MSFS limitations, it's unfortunately not possible to take into account the 
visibility conditions, so your PAX will be amazed at the scenery as soon as you hit 1500 ft AGL.
10. **Steep pitch is uncomfortable**. Don't pitch too much even when it wouldn't otherwise impact your vertical velocity.
11. **Hard landings are uncomfortable**, and rebounds too. On the other hand, keep in mind the first point on this list; a 
very long smooth landing could net you a worst review than a harder but shorter landing. 
12. **Emergencies are a mixed bag**. And a pain to track and rate efficiently in MSFS. At the moment, they have no impact on
your passengers. Sure, an engine fire is stressful, but surviving it is such a relief that it kind of balances out. 
13. **Crashing is uncool**. No "reward" for crashing, no death count, the flight will just stop being tracked and no report will be shown.
14. **PAX aren't aware of everything**. They don't care for delays, they don't care if you don't follow ATC's instructions, they don't 
know you have only 5 minutes of fuel left, they don't fear approaching storms, they don't care for lights... Implementing some of those would make
the simulation way too complex and less compatible with other software.
15. **PAX are smart:** they always wear their seat belts, no matter what cockpit switch you flip or not. Smart or overly anxious, your call.
16. **PAX are resentful**. They won't forget any mistake you made, and once they've panicked they won't bother you with mundane concerns anymore. At least until next flight.
Perhaps they're not *that* resentful, as they might just forgive you and give you another chance later and even a positive review.
17. **No parachuting... yet?** Your PAX expect to land WITH the plane. You could interrupt the flight while still in
mid-air but landings are a big part of the evaluation process and as such it could impact negatively your experience. Will
probably implemented in a future update, though.


Your average city-dwellers are more sensitive than bush trip travelers on some of those points. The number of passengers
also has an influence on some.

## Installation

Just unzip the archive in a folder of your choice. pax4props does **NOT** need to be installed in your Community folder 
(and it might even have a negative impact if you do install it there).

**dotnet 4.7.2 required**

## Usage

1. Launch pax4props and MSFS (the order shouldn't matter). 

2. Set the number of passengers on board, both in pax4props and MSFS. Weight doesn't have an influence. For reference,
Wikipedia states that *"Average adult human weight varies by continent from about 60 kg (130 lb) in Asia and Africa to about 80 kg (180 lb) in North America, with men on average weighing more than women. "*

3. Choose between regular flight and bush trip. Regular allows higher scores but require more finesse. "Bush PAX" are more resilient,
but the harsh conditions take a toll on their morale. You should definitely consider selecting it when flying to or from high altitudes airports, though.
Selecting "Bush trip" but flying from and to long runways with easy departures and approaches could otherwise be seen as cheating but who am I to judge you? 

4. When the plane is ready and not already airborne, click the "Start flight" button and fly.

5. When you have landed (preferably after taxiing to the ramp/parking/gate, click on "End flight" and read the flight
report. 

The number of passengers and type of flight are saved between sessions for an even shorter/lazier setup.

No interaction is needed during the flight. You might hear them voice their concern or awe.  You can also check their current status/mood by alt-tabbing to the pax4props
window during flight. There's no plan to implement a permanent visual overlay, for realism purposes: checking on them is a distraction and a possible danger. Focus on your
flying. They will let you know when they're really scared or angry.

## Roadmap

Planned features, without any guarantee or timeline:

1. Taking sim rate into account?
2. Profiles
3. Statistics
4. Export data
5. Parachuting (for the passengers only. You will have to stay on board).
6. Helicopters?
8. Language selection for voices?
8. more voiced reactions?
9. Automatic detection of PAX aboard plane. Might not always work for some third-party aircraft though.
10. Themes?


## Contributing

Anyone can help the project grow, even without any apparent skill.

1. **Submit bug reports.** Tell me what you did, what you expected, and what you got instead.
2. **Submit language corrections.** English is my second language (French is the first if you want to know) and while I've been consuming a lot of media in English
for a few decades now my grammar skills are still far from perfect. So please do correct me. Just keep in mind that most of the "PAX quotes" are supposed to be colloquial language,
and they may contain common errors.
3. **Submit your own voice files.** You don't need high-end recording equipment for that, they will be played at a relatively low volume in a noisy environment. Try to minimize
background noises. No birds chirping or cars honking behind you.
They should contain no specifics, keep in mind they should be compatible with all kind of planes and locations in the world. And keep them short.
4. **Submit suggestions and feature requests.** Keep in mind the core principles above.
5. **Talk about it.** Publicity, yay! 
6. **Just tell me that you like it.** Yeah it helps.


## Credits

- Based on [minisimconnect](https://github.com/bertrandpsi/minisimconnect) by bertrandpsi.

- Some panic screams by [Volonda](https://freesound.org/people/Volonda/).

- Additional code and art by fwisard.

## Licensing

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.