



### ~Filter using HTML form, radio buttons and reset button
#### A HTML from for filtering items

<form class="filter">
  <input class="btn btn-square" type="reset" value="×"/>
  <input class="btn" type="radio" name="frameworks" aria-label="Svelte"/>
  <input class="btn" type="radio" name="frameworks" aria-label="Vue"/>
  <input class="btn" type="radio" name="frameworks" aria-label="React"/>
</form>


### ~Filter without HTML form
#### Use this if you can't use a HTML form for some reason

<div class="filter">
  <input class="btn filter-reset" type="radio" name="metaframeworks" aria-label="All"/>
  <input class="btn" type="radio" name="metaframeworks" aria-label="Sveltekit"/>
  <input class="btn" type="radio" name="metaframeworks" aria-label="Nuxt"/>
  <input class="btn" type="radio" name="metaframeworks" aria-label="Next.js"/>
</div>


