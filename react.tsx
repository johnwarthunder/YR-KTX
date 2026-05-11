APP.TSX----------------------------------

{/* TODO: ide jön a Health komponens */}
<Health hp={selectedPokemon.health}/>
  
{/* TODO: itt kell mappelni a PokeBall-okat*/}
{pokemonsData.map(pokemon => <PokeBall key={pokemon.name} pokemon={pokemon}/>)}

CONTEXTPROVIDER.TSX----------------------------------

// TODO: itt kell a json-ból olvasni
useEffect(()=> {
    fetch("pokemons.json")
    .then(response => response.json())
    .then(apiData => {
        setPokemonsData(apiData)

        if(apiData.length > 0) {
            setSelectedPokemon(apiData[0])
        }
    })
},[])

HEALTH OR STAR RATING.TSX----------------------------------

const fullHeart = <i className="fa-solid fa-heart"></i>
const crackedHeart = <i className="fa-solid fa-heart-crack"></i>
const emptyHeart = <i className="fa-regular fa-heart"></i>

type HealthProps = {
    hp : number
}

const Health = ({hp}: HealthProps) => {
  const returnHealth = (index: number) => {
    if(Math.floor(hp) > index) return fullHeart
    if(hp > index) return crackedHeart
    return emptyHeart
  }

  return (
    <div className="health">
      {[...Array(5)].map((_, index) => returnHealth(index))}
    </div>
  )
}

export default Health


EXAM.CSS----------------------------------
/* TODO: CSS feladatok */
.pokeBallWrapper {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 10px;
}

@media (max-width: 600px) {
    .pokeBallWrapper {
        grid-template-columns: repeat(3, 1fr);
    }
}
