---
name: expense-report
description: Validate employee expense reports against company policy. Use when asked about expense submissions, reimbursement rules, or spending limits.
---

# Expense Report Skill

## Instructions

1. Read `references/POLICY.md` for reimbursement rules
2. Return thre results as pure JSON, e.g.:  
   {
       "status": "approved" | "rejected",
       "reason": "string"
   }
