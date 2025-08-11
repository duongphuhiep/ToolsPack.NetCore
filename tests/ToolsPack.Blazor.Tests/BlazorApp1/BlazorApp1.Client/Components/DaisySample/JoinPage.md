---
url: /daisy/JoinPage
---





### ~Join
<div class="join">
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
</div>




### ~Group items vertically
<div class="join join-vertical">
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
</div>




### ~Responsive: it's vertical on small screen and horizontal on large screen
<div class="join join-vertical lg:join-horizontal">
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
  <button class="btn join-item">Button</button>
</div>




### ~With extra elements in the group
#### Even if join-item is not a direct child of the group, it still gets the style

<div class="join">
  <div>
    <div>
      <input class="input join-item w-[5.3rem] md:w-52" placeholder="Search"/>
    </div>
  </div>
  <select class="select join-item w-[5.8rem] md:w-auto">
    <option disabled selected>Filter</option>
    <option>Sci-fi</option>
    <option>Drama</option>
    <option>Action</option>
  </select>
  <div class="indicator">
    <span class="indicator-item badge badge-secondary">new</span>
    <button class="btn join-item">Search</button>
  </div>
</div>




### ~Custom border radius
<div class="join">
  <input class="input join-item w-36 lg:w-52" placeholder="Email"/>
  <button class="btn join-item rounded-r-full">Subscribe</button>
</div>




### ~Join radio inputs with btn style
<div class="join">
  <input class="join-item btn" type="radio" name="options" aria-label="Radio 1" />
  <input class="join-item btn" type="radio" name="options" aria-label="Radio 2" />
  <input class="join-item btn" type="radio" name="options" aria-label="Radio 3" />
</div>


