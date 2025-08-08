



> :INFO:
>
> Also see [accordion](/components/accordion/) examples

### ~Collapse with focus

#### This collapse works with focus. When div loses focus, it gets closed

<div tabindex="0" class="collapse bg-base-100 border border-base-300">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~Collapse with checkbox

#### This collapse works with checkbox instead of focus. It needs to get clicked again to get closed.

<div class="collapse bg-base-100 border border-base-300">
  <input type="checkbox" />
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~ Collapse using details and summary tag

#### collapse-open and collapse-close doesn't work with this method. You can add/remove open attribute to the details instead

<details class="collapse bg-base-100 border border-base-300">
  <summary class="collapse-title font-semibold">How do I create an account?</summary>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</details>



<div class="alert text-sm mt-4">
  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="stroke-current shrink-0 w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
  <div>Using <code>&lt;details&gt;</code> tag, we can't have animations because <code>&lt;details&gt;</code> tag doesn't allow CSS transitions.</div>
</div>

### ~Without border and background color

<div tabindex="0" class="collapse">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~With arrow icon

<div tabindex="0" class="collapse bg-base-100 border border-base-300 collapse-arrow">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~With arrow plus/minus icon

<div tabindex="0" class="collapse bg-base-100 border border-base-300 collapse-plus">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~Force open

<div tabindex="0" class="collapse collapse-open bg-base-100 border border-base-300">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~Force close

<div tabindex="0" class="collapse collapse-close bg-base-100 border border-base-300">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~Custom colors for collapse that works with focus

#### Use Tailwind CSS `group` and `group-focus` utilities to apply style when parent div is focused

<div tabindex="0" class="collapse bg-primary text-primary-content focus:bg-secondary focus:text-secondary-content">
  <div class="collapse-title font-semibold">How do I create an account?</div>
  <div class="collapse-content text-sm">Click the "Sign Up" button in the top right corner and follow the registration process.</div>
</div>



### ~Custom colors for collapse that works with checkbox

#### Use Tailwind CSS `peer` and `peer-checked` utilities to apply style when sibling checkbox is checked

<div class="collapse bg-base-100 border border-base-300">
  <input type="checkbox" class="peer" />
  <div class="collapse-title bg-primary text-primary-content peer-checked:bg-secondary peer-checked:text-secondary-content">
    How do I create an account?
  </div>
  <div class="collapse-content bg-primary text-primary-content peer-checked:bg-secondary peer-checked:text-secondary-content">
    Click the "Sign Up" button in the top right corner and follow the registration process.
  </div>
</div>


