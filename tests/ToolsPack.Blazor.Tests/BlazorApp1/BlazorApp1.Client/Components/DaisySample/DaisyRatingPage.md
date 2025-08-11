---
url: /daisy/DaisyRatingPage
---





> :INFO:
>
> Items in each rating should have unique `name` attributes to avoid conflicts with other ratings on the same page.

### ~Rating
<div class="rating">
  <input type="radio" name="rating-1" class="mask mask-star" aria-label="1 star" />
  <input type="radio" name="rating-1" class="mask mask-star" aria-label="2 star" />
  <input type="radio" name="rating-1" class="mask mask-star" aria-label="3 star" />
  <input type="radio" name="rating-1" class="mask mask-star" aria-label="4 star" />
  <input type="radio" name="rating-1" class="mask mask-star" aria-label="5 star" />
</div>




### ~Read-only Rating
<div class="rating">
  <div class="mask mask-star" aria-label="1 star"></div>
  <div class="mask mask-star" aria-label="2 star"></div>
  <div class="mask mask-star" aria-label="3 star" aria-current="true"></div>
  <div class="mask mask-star" aria-label="4 star"></div>
  <div class="mask mask-star" aria-label="5 star"></div>
</div>





### ~mask-star-2 with warning color
<div class="rating">
  <input type="radio" name="rating-2" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
  <input type="radio" name="rating-2" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
  <input type="radio" name="rating-2" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
  <input type="radio" name="rating-2" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
  <input type="radio" name="rating-2" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
</div>




### ~mask-heart with multiple colors
<div class="gap-1 rating">
  <input type="radio" name="rating-3" class="mask mask-heart bg-red-400" aria-label="1 star" />
  <input type="radio" name="rating-3" class="mask mask-heart bg-orange-400" aria-label="2 star" checked="checked" />
  <input type="radio" name="rating-3" class="mask mask-heart bg-yellow-400" aria-label="3 star" />
  <input type="radio" name="rating-3" class="mask mask-heart bg-lime-400" aria-label="4 star" />
  <input type="radio" name="rating-3" class="mask mask-heart bg-green-400" aria-label="5 star" />
</div>




### ~mask-star-2 with green-500 color
<div class="rating">
  <input type="radio" name="rating-4" class="bg-green-500 mask mask-star-2" aria-label="1 star" />
  <input type="radio" name="rating-4" class="bg-green-500 mask mask-star-2" aria-label="2 star" checked="checked" />
  <input type="radio" name="rating-4" class="bg-green-500 mask mask-star-2" aria-label="3 star" />
  <input type="radio" name="rating-4" class="bg-green-500 mask mask-star-2" aria-label="4 star" />
  <input type="radio" name="rating-4" class="bg-green-500 mask mask-star-2" aria-label="5 star" />
</div>




### ~Sizes
<div class="flex flex-col gap-2 items-center">
  <div class="rating rating-xs">
    <input type="radio" name="rating-5" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
    <input type="radio" name="rating-5" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
    <input type="radio" name="rating-5" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
    <input type="radio" name="rating-5" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
    <input type="radio" name="rating-5" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
  </div>
  <div class="rating rating-sm">
    <input type="radio" name="rating-6" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
    <input type="radio" name="rating-6" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
    <input type="radio" name="rating-6" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
    <input type="radio" name="rating-6" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
    <input type="radio" name="rating-6" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
  </div>
  <div class="rating rating-md">
    <input type="radio" name="rating-7" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
    <input type="radio" name="rating-7" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
    <input type="radio" name="rating-7" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
    <input type="radio" name="rating-7" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
    <input type="radio" name="rating-7" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
  </div>
  <div class="rating rating-lg">
    <input type="radio" name="rating-8" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
    <input type="radio" name="rating-8" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
    <input type="radio" name="rating-8" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
    <input type="radio" name="rating-8" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
    <input type="radio" name="rating-8" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
  </div>
  <div class="rating rating-xl">
    <input type="radio" name="rating-9" class="mask mask-star-2 bg-orange-400" aria-label="1 star" />
    <input type="radio" name="rating-9" class="mask mask-star-2 bg-orange-400" aria-label="2 star" checked="checked" />
    <input type="radio" name="rating-9" class="mask mask-star-2 bg-orange-400" aria-label="3 star" />
    <input type="radio" name="rating-9" class="mask mask-star-2 bg-orange-400" aria-label="4 star" />
    <input type="radio" name="rating-9" class="mask mask-star-2 bg-orange-400" aria-label="5 star" />
  </div>
</div>




### ~with `rating-hidden`
#### `rating-hidden` is a hidden radio at the start to allow users to remove their rating.

<div class="rating rating-lg">
  <input type="radio" name="rating-10" class="rating-hidden" aria-label="clear" />
  <input type="radio" name="rating-10" class="mask mask-star-2" aria-label="1 star" />
  <input type="radio" name="rating-10" class="mask mask-star-2" aria-label="2 star" checked="checked" />
  <input type="radio" name="rating-10" class="mask mask-star-2" aria-label="3 star" />
  <input type="radio" name="rating-10" class="mask mask-star-2" aria-label="4 star" />
  <input type="radio" name="rating-10" class="mask mask-star-2" aria-label="5 star" />
</div>




### ~half stars
<div class="rating rating-lg rating-half">
  <input type="radio" name="rating-11" class="rating-hidden" aria-label="clear" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-1" aria-label="0.5 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-2" aria-label="1 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-1" aria-label="1.5 star" checked="checked" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-2" aria-label="2 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-1" aria-label="2.5 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-2" aria-label="3 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-1" aria-label="3.5 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-2" aria-label="4 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-1" aria-label="4.5 star" />
  <input type="radio" name="rating-11" class="bg-green-500 mask mask-star-2 mask-half-2" aria-label="5 star" />
</div>


