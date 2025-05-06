Lifecycle Diagram – EasySave

This diagram represents the "lifecycle of a backup job" in the EasySave application. It illustrates the various states a backup can go through, from its creation to completion, including error handling and retries.

Description of States

- [Start]: Entry point of the backup process.
- Initialized: The backup job has been created and configured.
- Pending: The job is waiting to be launched (scheduled or triggered manually).
- In_Progress: The backup is currently being executed.
- Completed: The backup has been successfully completed.
- Failed: An error occurred during the backup.
- Retry: The system attempts to resume a failed backup.

Transitions

- Once the job is "initialized", it moves to "Pending" state.
- As soon as the launch is triggered, it transitions to the "In_Progress" state.
- If the backup succeeds, it transitions to "completed", then to the "[End]" of the cycle.
- In case of failure, the process transitions to the "failed" state. It can then either:
  - End if no retry is performed.
  - Be resumed through a retry, returning to the "In_Progress" state.

Purpose

This diagram is essential to model the dynamic behavior of the EasySave system, particularly error handling, automatic retries, and the real-time status tracking of each backup job.

This diagram was created for version 1.0 of EasySave as part of Deliverable 1.
